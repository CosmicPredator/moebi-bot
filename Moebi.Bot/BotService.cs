using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Moebi.Bot;

/// <summary>
/// Service responsible for managing the Discord bot lifecycle (startup, command registration, and shutdown).
/// </summary>
/// <param name="client">The Discord socket client.</param>
/// <param name="configuration">Application configuration settings.</param>
/// <param name="interactionService">Service for managing interactions (slash commands, etc.).</param>
/// <param name="interactionHandler">Handler for initializing and processing interactions.</param>
public class BotService(DiscordSocketClient client,
    IConfiguration configuration,
    InteractionService interactionService,
    InteractionHandler interactionHandler) : IHostedService
{
    private readonly ILogger _contextLogger = Log.ForContext<InteractionHandler>();
    
    /// <summary>
    /// Starts the bot service, logs in to Discord, initializes handlers, and registers commands.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to handle task cancellation.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        interactionService.Log += LogMapper.SerilogMapper;
        client.Log += LogMapper.SerilogMapper;

        await interactionHandler.InitializeAsync();

        client.Ready += async () =>
        {
            var guildId = configuration["Discord:GuildId"];
            if (string.IsNullOrWhiteSpace(guildId))
            {
                _contextLogger.Error("No guild ID was provided.");
                await StopAsync(cancellationToken);
            }
            //await interactionService.RegisterCommandsToGuildAsync(ulong.Parse(guildId!));
            await interactionService.RegisterCommandsGloballyAsync(deleteMissing: true);
            
            _contextLogger.Information("Logged in as {user}", client.CurrentUser.Username);
            _contextLogger.Information("Socket latency: {latency}ms", client.Latency);

            await Task.CompletedTask;
        };
        
        await client.LoginAsync(TokenType.Bot, configuration["Discord:BotToken"]);
        await client.StartAsync();
        
        await client.SetGameAsync("Anime", null, ActivityType.Watching);
    }

    /// <summary>
    /// Stops the bot service, disconnects and logs out from Discord.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to handle task cancellation.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await client.StopAsync();
        await client.LogoutAsync();
    }
}