using Microsoft.Extensions.Hosting;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using HutaoWaifuBot;
using HutaoWaifuBot.Handlers;
using HutaoWaifuBot.Commands;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

var dbPath = "Data/hutao_waifu_bot.db";
var dbManager = new DatabaseManager(dbPath);

// IMPORTANT: comment this line if it's not the first time running the code
// CharacterSeedData.SeedCharacters(dbManager); // Only executed on initialization

builder.Services
    .AddDiscordGateway(options =>
    {
        options.Intents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContent;
    })
    .AddGatewayEventHandlers(typeof(Program).Assembly);

var host = builder.Build()
    .UseGatewayEventHandlers();

await host.RunAsync();
