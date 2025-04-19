using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moebi.Bot;
using Moebi.Bot.Anilist.Repositories;
using Moebi.Bot.Anilist;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

ConfigureLogger(configuration);

var scopedLogger = Log.ForContext("SourceContext", nameof(Moebi));
scopedLogger.Debug("Starting MoebiBot...");

var discordSocketConfig = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.AllUnprivileged,
    LogGatewayIntentWarnings = false,
    LogLevel = LogSeverity.Debug
};

var botHost = Host.CreateDefaultBuilder()
    .ConfigureServices(serviceCollection =>
    {
        serviceCollection.AddHttpClient<IAnilistClient, AnilistClient>();
        serviceCollection
            .AddSingleton(discordSocketConfig)
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(provider => 
                new InteractionService(provider.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .AddSingleton<MediaRepository>()
            .AddSingleton<CharacterRepository>()
            .AddHostedService<BotService>();
    })
    .UseSerilog(Log.ForContext<IHost>())
    .Build();

await botHost.RunAsync();

void ConfigureLogger(IConfiguration config)
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .CreateLogger();
    Log.ForContext<ILogger>().Information("Logger configured");
}