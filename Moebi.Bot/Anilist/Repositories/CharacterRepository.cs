using Moebi.Bot.Anilist.Models.Character;

namespace Moebi.Bot.Anilist.Repositories;

/// <summary>
/// Repository for interacting with AniList's character-related GraphQL queries.
/// </summary>
public class CharacterRepository(IAnilistClient client)
{
    /// <summary>
    /// Searches for characters based on a search query.
    /// </summary>
    /// <param name="searchQuery">The search term to search for characters.</param>
    /// <param name="pageNum">The page number to retrieve results from (default is 1).</param>
    /// <returns>A task representing the asynchronous operation, with the search results wrapped in a <see cref="CharacterSearchModel"/> instance.</returns>
    public async Task<CharacterSearchModel?> SearchCharacterAsync(string searchQuery, int pageNum = 1)
    {
        var vars = new Dictionary<string, object>
        {
            { "pageNum", pageNum },
            { "search", searchQuery }
        };
        var response = await client.PostAsync<CharacterSearchModel>(Queries.CharacterSearchQuery, vars);
        return response;
    }

    public async Task<CharacterDetailModel?> GetCharacterDetailsAsync(int id)
    {
        var vars = new Dictionary<string, int>
        {
            { "id", id }
        };
        var response = await client.PostAsync<CharacterDetailModel>(Queries.CharacterDetailQuery, vars);
        return response;
    }
}