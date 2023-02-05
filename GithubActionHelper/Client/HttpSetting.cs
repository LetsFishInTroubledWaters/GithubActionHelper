namespace GithubActionHelper.Client;

public class HttpSetting
{
    public GithubClientSetting Github { get; set; }
    
    public WechatClientSetting Wechat { get; set; }
    
    public class GithubClientSetting
    {
        public string Url { get; set; }
        
        public string Token { get; set; }
    }
    
    public class WechatClientSetting
    {
        public string Url { get; set; }
        
        public string Key { get; set; }
    }
}