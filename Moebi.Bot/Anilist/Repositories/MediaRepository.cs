using Moebi.Bot.Anilist.Models;
using Moebi.Bot.Anilist.Models.Media;

namespace Moebi.Bot.Anilist.Repositories;

public class MediaRepository(IAnilistClient client)
{
    public async Task<MediaDetailModel?> GetMediaDetail(int id)
    {
        var vars = new Dictionary<string, int>
        {
            { "id", id }
        };
        var response = await client.PostAsync<MediaDetailModel>(Queries.MediaDetailQuery, vars);
        return response;
    }
    
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