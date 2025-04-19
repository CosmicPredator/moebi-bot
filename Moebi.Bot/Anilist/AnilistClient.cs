using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Moebi.Bot.Anilist.Models;
using Serilog;

namespace Moebi.Bot.Anilist;

public class AnilistClient(HttpClient httpClient) : IAnilistClient
{
    private const string ApiEndpoint = "https://graphql.anilist.co";
    private readonly ILogger _contextLogger = Log.ForContext<AnilistClient>();
    private readonly JsonSerializerOptions _jsonDeserializeOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
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

    private string ObjectToJson(object obj)
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptions.Default);
    }
}