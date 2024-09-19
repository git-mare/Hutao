using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;

using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Services.ApplicationCommands;
using NetCord.Gateway;
using HutaoWaifuBot;
using NetCord;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
	.AddDiscordGateway(static options =>
	{
		options.Intents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContent;
	}).AddApplicationCommands<SlashCommandInteraction, SlashCommandContext>();

IHost host = builder.Build()
	.AddApplicationCommandModule<SlashCommandContext>(typeof(WaifuCommand))
	.UseGatewayEventHandlers();

await host.RunAsync();
