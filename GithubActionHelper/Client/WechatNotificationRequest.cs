using Newtonsoft.Json;

namespace GithubActionHelper.Client;

public class WechatNotificationRequest
{
    [JsonProperty("msgtype")]
    public string Type { get; set; }

    [JsonProperty("text")]
    public WechatText Text { get; set; }
    
    public class WechatText
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        
        [JsonProperty("mentioned_mobile_list")]
        public List<string> Mentioned { get; set; }
    }
}