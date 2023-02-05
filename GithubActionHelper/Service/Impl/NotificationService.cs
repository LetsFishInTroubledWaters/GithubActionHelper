using GithubActionHelper.Client;

namespace GithubActionHelper.Service.Impl;

public class NotificationService : INotificationService
{
    private readonly IWechatClient _wechatClient;

    public NotificationService(IWechatClient wechatClient)
    {
        _wechatClient = wechatClient;
    }

    public async Task SendNotification(Notification notification)
    {
        var wechatNotificationRequest = new WechatNotificationRequest
        {
            Type = "text",
            Text = new WechatNotificationRequest.WechatText
            {
                Content = $"{notification.Repo}构建失败，请及时处理!",
                Mentioned = new List<string> { notification.Mentioned }
            }
        };
        await _wechatClient.PostMessageAsync<object>(wechatNotificationRequest);
    }
}