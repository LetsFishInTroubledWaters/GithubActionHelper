namespace GithubActionHelper.Service;

public interface INotificationService
{
    Task SendNotification(List<Notification> notifications);
    
    Task SendNotification(Notification notification);
}