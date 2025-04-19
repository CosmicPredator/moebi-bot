using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Moebi.Bot.Anilist.Models;
using Serilog;

namespace Moebi.Bot.Anilist;

public class AnilistClient(HttpClient httpClient) : IAnilistClient
{
    /// <summary>
    /// The endpoint URL for the AniList GraphQL API.
    /// </summary>
    private const string ApiEndpoint = "https://graphql.anilist.co";
    /// <summary>
    /// Logger scoped to the AnilistClient class for logging purposes.
    /// </summary>
    private readonly ILogger _contextLogger = Log.ForContext<AnilistClient>();
    /// <summary>
    /// JSON serializer options to control serialization behavior.
    /// </summary>
    private readonly JsonSerializerOptions _jsonDeserializeOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    /// <summary>
    /// Sends a GraphQL request to AniList API and deserializes the response into a model.
    /// </summary>
    /// <typeparam name="T">The type of the response model, which must derive from <see cref="Model"/>.</typeparam>
    /// <param name="query">The GraphQL query string to send to the API.</param>
    /// <param name="variables">The variables to be included in the GraphQL query.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="T"/>.</returns>
    public async Task<T?> PostAsync<T>(string query, object variables) where T : Model
    {
        var payload = new
        {
            query,
            variables
        };
        var payloadString = new StringContent(
            ObjectToJson(payload), 
            Encoding.UTF8, 
            "application/json");
        var request = await httpClient.PostAsync(ApiEndpoint, payloadString);
        if (!request.IsSuccessStatusCode)
        {
            _contextLogger.Warning("The request returned {0} error code", request.StatusCode);
            _contextLogger.Debug(await request.Content.ReadAsStringAsync());
        }
        
        var responseString = await request.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseString, _jsonDeserializeOptions);
    }

    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    private string ObjectToJson(object obj)
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptions.Default);
    }
}