using Moebi.Bot.Anilist.Models.Character;

namespace Moebi.Bot.Anilist.Repositories;

public class CharacterRepository(IAnilistClient client)
{
    public async Task<CharacterSearchModel?> SearchCharacterAsync(string searchQuery, int pageNUm = 1)
    {
        var vars = new Dictionary<string, object>
        {
            { "pageNum", pageNUm },
            { "search", searchQuery }
        };
        var response = await client.PostAsync<CharacterSearchModel>(Queries.CharacterSearchQuery, vars);
        return response;
    }
}