using System.Reflection;
using Discord.Interactions;
using Discord.WebSocket;
using Serilog;

namespace Moebi.Bot;

/// <summary>
/// Handles the initialization and execution of Discord interaction commands (such as slash commands).
/// </summary>
/// <param name="client">The Discord socket client.</param>
/// <param name="provider">The service provider for resolving dependencies.</param>
/// <param name="interactionService">Service for managing and executing interaction modules.</param>
public class InteractionHandler (DiscordSocketClient client,
    IServiceProvider provider,
    InteractionService interactionService)
{
    private readonly ILogger _contextLogger = Log.ForContext<InteractionHandler>();
    
    /// <summary>
    /// Initializes the interaction handler by adding command modules and subscribing to interaction events.
    /// </summary>
    public async Task InitializeAsync()
    {
        await interactionService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), provider);
        client.InteractionCreated += ClientOnInteractionCreated;
    }

    /// <summary>
    /// Event handler for when a Discord interaction is created (e.g., a slash command is used).
    /// </summary>
    /// <param name="arg">The socket interaction received from Discord.</param>
    private async Task ClientOnInteractionCreated(SocketInteraction arg)
    {
        try
        {
            SocketInteractionContext context = new(client, arg);
            await interactionService.ExecuteCommandAsync(context, provider);
        }
        catch (Exception e)
        {
            _contextLogger.Error(e, e.Message);
        }
    }
}