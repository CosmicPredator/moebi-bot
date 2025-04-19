using Moebi.Bot.Anilist.Models;

namespace Moebi.Bot.Anilist;

/// <summary>
/// Interface for communicating with the AniList API, providing a method to send GraphQL requests.
/// </summary>
public interface IAnilistClient
{
    /// <summary>
    /// Sends a GraphQL request to AniList and returns the response as a strongly-typed model.
    /// </summary>
    /// <typeparam name="T">The type of the response model, which must derive from <see cref="Model"/>.</typeparam>
    /// <param name="query">The GraphQL query to be sent.</param>
    /// <param name="variables">The variables to be included in the query.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="T"/>.</returns>
    Task<T?> PostAsync<T>(string query, object variables) where T : Model;
}