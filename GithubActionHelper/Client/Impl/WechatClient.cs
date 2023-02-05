using System.Text;
using Newtonsoft.Json;

namespace GithubActionHelper.Client.Impl;

public class WechatClient : IWechatClient
{
    private readonly HttpClient _httpClient;

    private readonly HttpSetting _httpSetting;

    public WechatClient(HttpClient httpClient, HttpSetting httpSetting)
    {
        _httpClient = httpClient;
        _httpSetting = httpSetting;
    }

    public async Task<T?> PostMessageAsync<T>(object body) where T : class
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"cgi-bin/webhook/send?key={_httpSetting.Wechat.Key}");
        requestMessage.Content = new StringContent(
            JsonConvert.SerializeObject(body), 
            Encoding.UTF8,
            "application/json");

        var result = await _httpClient.ExecuteAsync<T>(requestMessage);
        return result;
    }
}