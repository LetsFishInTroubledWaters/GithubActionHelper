using GithubActionHelper.Client;

namespace GithubActionHelper.Service.Impl;

public class NotificationService : INotificationService
{
    private readonly IWechatClient _wechatClient;
    
    private readonly ILogger<NotificationService> _logger;


    public NotificationService(IWechatClient wechatClient, ILogger<NotificationService> logger)
    {
        _wechatClient = wechatClient;
        _logger = logger;
    }

    public async Task SendNotification(Notification notification)
    {
        var card = new CardNotificationRequest
        {
            Template = new CardNotificationRequest.CardTemplate
            {
                Source = new CardNotificationRequest.CardTemplate.CardSource
                {
                    Desc = "Workflow Notification"
                },
                Title = new CardNotificationRequest.CardTemplate.CardDesc
                {
                    Title = "Workflow失败",
                    Desc = notification.Name
                },
                EmphasisContent = new CardNotificationRequest.CardTemplate.CardDesc
                {
                    Title = notification.Repo.Split("_").Last(),
                    Desc = $"{notification.Branch}分支"
                },
                Content = new List<CardNotificationRequest.CardTemplate.CardContent>
                {
                    new CardNotificationRequest.CardTemplate.CardContent()
                    {
                        KeyName = "提交人",
                        Value = notification.Author
                    },
                    new CardNotificationRequest.CardTemplate.CardContent()
                    {
                        KeyName = "触发时间",
                        Value = notification.RunTime.ToOffset(TimeSpan.FromHours(8)).ToString()
                    }
                },
                JumpList = new List<CardNotificationRequest.CardTemplate.Jump>
                {
                    new CardNotificationRequest.CardTemplate.Jump
                    {
                        Title = "点击查看详情",
                        Url = notification.Url
                    }
                },
                CardAction = new CardNotificationRequest.CardTemplate.Action()
                {
                    Type = 1,
                    Url = notification.Url
                }
            }
        };
        var wechatNotificationRequest = new TextNotificationRequest
        {
            Text = new TextNotificationRequest.WechatText
            {
                Content = $"{notification.Repo}构建失败，请及时处理!",
                Mentioned = new List<string> { notification.Mentioned }
            }
        };
        await _wechatClient.PostMessageAsync<object>(card);
        await _wechatClient.PostMessageAsync<object>(wechatNotificationRequest);
    }
}