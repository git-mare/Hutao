using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using NetCord.Gateway;
using NetCord.Services;
using NetCord.Services.Commands;

using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.Commands;

using HutaoWaifuBot;
using HutaoWaifuBot.Database;
using HutaoWaifuBot.Commands;

var builder = Host.CreateApplicationBuilder(args);

var dbPath = "Database/hutaowaifubot.db";

builder.Services
    .AddDbContext<WaifuBotContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));

builder.Services
    .AddDiscordGateway(options =>
    {
        options.Intents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContent;
    })
    .AddCommands<CommandContext>();

var host = builder.Build();
 .AddCommand<CommandContext>(["waifu", "w"], () => new WaifuCommand("Data Source=Database/hutaowaifubot.db"))
 .AddCommand<CommandContext>(["ping"], () => "Pong!")
 .AddModules(typeof(Program).Assembly)
 .UseGatewayEventHandlers();

using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WaifuBotContext>();
    context.Database.Migrate();
    CharacterSeedData.SeedCharacters(context);
}

await host.RunAsync();
