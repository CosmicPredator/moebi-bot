using Discord.Interactions;
using Moebi.Bot.Anilist.Models;
using Moebi.Bot.Anilist.Repositories;
using Moebi.Bot.Common;
using Serilog;

namespace Moebi.Bot.CommandHandlers;

[Group("character", "Set of commands related to character.")]
public class Character(CharacterRepository characterRepository) : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ILogger _contextLogger = Log.ForContext<Character>();

    [SlashCommand("search", "Searches character")]
    public async Task HandleSearchAsync(string searchQuery)
    {
        _contextLogger.Information("Searching character with query: {0}", searchQuery);
        await DeferAsync();
        try
        {
            var searchResult = await characterRepository.SearchCharacterAsync(searchQuery);
            if (searchResult is null || searchResult.data.Page.characters.Length == 0)
            {
                await FollowupAsync($"No character found with the name **{searchQuery}**", ephemeral: true);
                return;
            }
            await FollowupAsync(
                embed: Embeds.CharacterSearchEmbed(ref searchResult, searchQuery),
                components: Components.CharacterSearchSelectMenu(ref searchResult));
        }
        catch (Exception ex)
        {
            _contextLogger.Error(ex, "Error while performing character search command.");
            await FollowupAsync("Something went wrong.", ephemeral: true);
        }
    }
}