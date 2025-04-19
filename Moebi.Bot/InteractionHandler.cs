using System.Reflection;
using Discord.Interactions;
using Discord.WebSocket;
using Serilog;

namespace Moebi.Bot;

public class InteractionHandler (DiscordSocketClient client,
    IServiceProvider provider,
    InteractionService interactionService)
{
    private readonly ILogger _contextLogger = Log.ForContext<InteractionHandler>();
    
    public async Task InitializeAsync()
    {
        await interactionService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), provider);
        client.InteractionCreated += ClientOnInteractionCreated;
    }

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