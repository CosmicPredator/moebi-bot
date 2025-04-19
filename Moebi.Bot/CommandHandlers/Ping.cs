using Discord.Interactions;
using Serilog;

namespace Moebi.Bot.CommandHandlers;

public class Ping : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ILogger _contextLogger = Log.ForContext<Ping>();

    [SlashCommand("ping", "Pings the bot.")]
    public async Task HandlePingAsync()
    {
        var latency = Context.Client.Latency;
        await RespondAsync(embed: Common.Embeds.PingEmbed(ref latency));
    }
}