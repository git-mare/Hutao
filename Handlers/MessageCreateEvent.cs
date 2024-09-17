using Microsoft.Extensions.Logging;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Rest;
using HutaoWaifuBot.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCord;

namespace HutaoWaifuBot.Handlers;

[GatewayEvent(nameof(GatewayClient.MessageCreate))]
public class MessageCreateHandler : IGatewayEventHandler<Message>
{
    private readonly RestClient _client;
    private readonly ILogger<MessageCreateHandler> _logger;
    private readonly Dictionary<string, ICommand> _commands;

    public MessageCreateHandler(RestClient client, ILogger<MessageCreateHandler> logger)
    {
        _client = client;
        _logger = logger;

        _commands = new Dictionary<string, ICommand>
        {
            { "ping", new PingCommand() },
            { "waifu", new WaifuCommand("Data Source=Data/hutao_waifu_bot.db") },
            { "w", new WaifuCommand("Data Source=Data/hutao_waifu_bot.db") }
        };
    }

    public async ValueTask HandleAsync(Message message)
    {
        if (message.Content.StartsWith("."))
        {
            var commandKey = message.Content.Split(' ')[0].Substring(1).ToLower();

            if (_commands.TryGetValue(commandKey, out var command))
            {
                _logger.LogInformation("Executing command: {}", commandKey);
                await command.ExecuteAsync(message, _client);
            }
            else
            {
                _logger.LogWarning("Command not found: {}", commandKey);
            }
        }
        else
        {
            _logger.LogInformation("Message: {}", message.Content);
        }
    }
    public class ButtonInteractionHandler : IGatewayEventHandler<MessageComponentInteraction>
    {
        private readonly RestClient _client;

        public ButtonInteractionHandler(RestClient client)
        {
            _client = client;
        }

        public async ValueTask HandleAsync(MessageComponentInteraction interaction)
        {
            if (interaction.Data.CustomId == "Claim")
            {
                await _client.SendMessageAsync(interaction.Channel.Id, $"<@{interaction.User.Id}> coletou!");
            }
        }
    }
}
