namespace GithubActionHelper.Service;

public interface INotificationService
{
    Task SendNotification(Notification notification);
}