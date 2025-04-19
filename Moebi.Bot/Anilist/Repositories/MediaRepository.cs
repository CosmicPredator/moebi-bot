using Moebi.Bot.Anilist.Models;
using Moebi.Bot.Anilist.Models.Media;

namespace Moebi.Bot.Anilist.Repositories;

/// <summary>
/// Repository for interacting with AniList's media-related GraphQL queries.
/// </summary>
public class MediaRepository(IAnilistClient client)
{
    /// <summary>
    /// Retrieves detailed information about a specific media (anime or manga) by its ID.
    /// </summary>
    /// <param name="id">The ID of the media to retrieve details for.</param>
    /// <returns>A task representing the asynchronous operation, with the media details wrapped in a <see cref="MediaDetailModel"/> instance.</returns>
    public async Task<MediaDetailModel?> GetMediaDetail(int id)
    {
        var vars = new Dictionary<string, int>
        {
            { "id", id }
        };
        var response = await client.PostAsync<MediaDetailModel>(Queries.MediaDetailQuery, vars);
        return response;
    }
    
    /// <summary>
    /// Searches for media (anime or manga) based on a search query and media type.
    /// </summary>
    /// <param name="mediaType">The type of media to search for (e.g., anime, manga).</param>
    /// <param name="searchQuery">The search term to search for media.</param>
    /// <param name="pageNum">The page number to retrieve results from (default is 1).</param>
    /// <returns>A task representing the asynchronous operation, with the search results wrapped in a <see cref="MediaSearchModel"/> instance.</returns>
    public async Task<MediaSearchModel?> SearchMedia(MediaType mediaType, string searchQuery, int pageNum = 1)
    {
        var vars = new Dictionary<string, object>
        {
            ["search"] = searchQuery,
            ["pageNum"] = pageNum,
            ["mediaType"] = mediaType.ToString()
        };
        var response = await client.PostAsync<MediaSearchModel>(Queries.MediaSearchQuery, vars);
        return response;
    }
}