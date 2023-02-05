using System.Net.Mime;
using Newtonsoft.Json;

namespace GithubActionHelper.Client;

public static class HttpClientExtension
{
    public static async Task<T?> ExecuteAsync<T>(this HttpClient httpClient,  HttpRequestMessage requestMessage) where T : class
    {
        var resp = await httpClient.SendAsync(requestMessage);

        if (resp.Content.Headers.ContentType is { MediaType: MediaTypeNames.Application.Json })
        {
            return JsonConvert.DeserializeObject<T>(await resp.Content.ReadAsStringAsync());
        }
        
        throw new Exception("Client connect error");
    }
}