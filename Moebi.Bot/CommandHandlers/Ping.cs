using Discord.Interactions;
using Serilog;

namespace Moebi.Bot.CommandHandlers;

/// <summary>
/// Provides a simple ping command to check the bot's responsiveness and latency.
/// </summary>
public class Ping : InteractionModuleBase<SocketInteractionContext>
{
    /// <summary>
    /// Handles the /ping command. Responds with the bot's current latency.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [SlashCommand("ping", "Pings the bot.")]
    public async Task HandlePingAsync()
    {
        var latency = Context.Client.Latency;
        await RespondAsync(embed: Common.Embeds.PingEmbed(ref latency));
    }
}