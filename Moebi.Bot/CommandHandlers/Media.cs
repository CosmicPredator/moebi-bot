using Discord.Interactions;
using Moebi.Bot.Anilist.Repositories;
using Moebi.Bot.Anilist.Models;
using Moebi.Bot.Common;
using Serilog;

namespace Moebi.Bot.CommandHandlers;

/// <summary>
/// Defines interaction commands related to AniList media (anime and manga).
/// </summary>
[Group("media", "Set of commands related to anime and manga.")]
public class Media(MediaRepository mediaRepository) : InteractionModuleBase<SocketInteractionContext>
{
    /// <summary>
    /// Logger scoped to the Media command handler.
    /// </summary>
    private readonly ILogger _contextLogger = Log.ForContext<Media>();

    
    /// <summary>
    /// Searches for anime or manga based on a user-provided query and media type.
    /// </summary>
    /// <param name="searchQuery">The search string provided by the user.</param>
    /// <param name="type">The type of media (Anime or Manga) to search for.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [SlashCommand("search", "Search anime or manga")]
    public async Task SearchAnimeAsync(string searchQuery, MediaType type)
    {
        _contextLogger.Information("Searching {0} with query {1}", type.ToString(), searchQuery);
        await DeferAsync();
        try
        {
            var searchResult = await mediaRepository.SearchMedia(type, searchQuery);
            if (searchResult is null || searchResult.data.Page.media.Length == 0)
            {
                await RespondAsync($"No media found with the name **{searchQuery}**", ephemeral: true);
                return;
            }
            await RespondAsync(
                embed: Embeds.MediaSearchEmbed(ref searchResult, searchQuery),
                components: Components.MediaSearchSelectMenu(ref searchResult));
        }
        catch (Exception ex)
        {
            _contextLogger.Error(ex, "Error while performing media search command.");
            await RespondAsync("Something went wrong.", ephemeral: true);
        }
    }
    
    /// <summary>
    /// Handles the selection of a media item from the search results menu.
    /// </summary>
    /// <param name="mediaId">The ID of the selected media item.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [ComponentInteraction("media_search_select", true)]
    public async Task HandleMediaSearchSelectAsync(string mediaId)
    {
        _contextLogger.Information("Getting media from provided media ID.");
        await DeferAsync();
        try
        {
            var mediaDetail = await mediaRepository.GetMediaDetail(Convert.ToInt32(mediaId));
            if (mediaDetail is null)
            {
                await RespondAsync("No media found with the given media ID.", ephemeral: true);
                return;
            }
            await ModifyOriginalResponseAsync((response) =>
            {
                response.Embed = Embeds.MediaDetailEmbed(ref mediaDetail);
            });
        }
        catch (Exception ex)
        {
            _contextLogger.Error(ex, "Error while performing media detail interaction.");
            await RespondAsync("Something went wrong.", ephemeral: true);
        }
    }
}