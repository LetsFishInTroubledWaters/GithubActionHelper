using GithubActionHelper.Service;

namespace GithubActionHelper.Worker;

public class NotificationRecord
{
    public WorkflowRun? WorkflowRun { get; set; }
    
    public DateTimeOffset LastNotificationTime { get; set; }

    public long NotificationTimes { get; set; }

    public void ResetWorkflowRun(WorkflowRun workflowRun)
    {
        WorkflowRun = workflowRun;
        NotificationTimes = 0;
        LastNotificationTime = DateTimeOffset.UtcNow;
    }

    public void AddNotificationTimes()
    {
        NotificationTimes += 1;
        LastNotificationTime = DateTimeOffset.UtcNow;
    }

    public bool ShouldNoticeAgain()
    {
        var oneHourNotification = NotificationTimes == 1 &&
                                  LastNotificationTime < DateTimeOffset.UtcNow.AddHours(-1) &&
                                  LastNotificationTime > DateTimeOffset.UtcNow.AddHours(-3);
        var threeHourNotification = NotificationTimes == 2 &&
                                  LastNotificationTime < DateTimeOffset.UtcNow.AddHours(-3) &&
                                  LastNotificationTime > DateTimeOffset.UtcNow.AddHours(-6);
        var sixHourNotification = NotificationTimes == 3 &&
                                  LastNotificationTime < DateTimeOffset.UtcNow.AddHours(-6) &&
                                  LastNotificationTime > DateTimeOffset.UtcNow.AddHours(-24);
        var oneDayNotification = NotificationTimes == 4 &&
                                  LastNotificationTime < DateTimeOffset.UtcNow.AddHours(-24);
        return oneHourNotification || threeHourNotification || sixHourNotification || oneDayNotification;
    }
}