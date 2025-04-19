using Moebi.Bot.Anilist.Models;

namespace Moebi.Bot.Anilist;

public interface IAnilistClient
{
    Task<T?> PostAsync<T>(string query, object variables) where T : Model;
}