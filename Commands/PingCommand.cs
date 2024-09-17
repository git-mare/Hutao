using NetCord.Gateway;
using NetCord.Rest;
using System.Windows.Input;
namespace HutaoWaifuBot.Commands;

public class PingCommand : ICommand
{
    public async Task ExecuteAsync(Message message, RestClient client)
    {
        await client.SendMessageAsync(message.ChannelId, "Pong!");
    }
}
