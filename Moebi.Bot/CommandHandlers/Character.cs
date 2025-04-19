using Discord.Interactions;
using Moebi.Bot.Anilist.Models;
using Moebi.Bot.Anilist.Repositories;
using Moebi.Bot.Common;
using Serilog;

namespace Moebi.Bot.CommandHandlers;

/// <summary>
/// Defines interaction commands related to AniList characters.
/// </summary>
[Group("character", "Set of commands related to character.")]
public class Character(CharacterRepository characterRepository) : InteractionModuleBase<SocketInteractionContext>
{
    /// <summary>
    /// Logger scoped to the Character command handler.
    /// </summary>
    private readonly ILogger _contextLogger = Log.ForContext<Character>();

    /// <summary>
    /// Searches for characters on AniList based on a user-provided query.
    /// </summary>
    /// <param name="searchQuery">The search string provided by the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    [ComponentInteraction("character_search_select", true)]
    public async Task HandleCharacterDetailAsync(int characterId)
    {
        _contextLogger.Information("Trying to get character details for id: {0}", characterId);
        await DeferAsync();
        try
        {
            var characterDetail = await characterRepository.GetCharacterDetailsAsync(characterId);
            if (characterDetail is null)
            {
                await FollowupAsync($"No character associated with id: **{characterId}**", ephemeral: true);
                return;
            }

            await ModifyOriginalResponseAsync((response) =>
            {
                response.Embed = Embeds.CharacterDetailEmbed(ref characterDetail);
            });
        }
        catch (Exception ex)
        {
            _contextLogger.Error(ex, "Error while performing character search command.");
            await FollowupAsync("Something went wrong.", ephemeral: true);
        }
    }
}