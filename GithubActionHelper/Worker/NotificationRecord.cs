using GithubActionHelper.Service;

namespace GithubActionHelper.Worker;

public class NotificationRecord
{
    public NotificationRecord(WorkflowRun workflowRun, GithubSetting.Repo repo)
    {
        WorkflowRun = workflowRun;
        Repo = repo;
        Branch = workflowRun.Branch;
        NotificationTimes = 0;
        UpdateTime = DateTimeOffset.UtcNow;
    }
    public GithubSetting.Repo Repo { get; set; }
    
    public string Branch { get; set; }
    
    public WorkflowRun WorkflowRun { get; set; }
    
    public DateTimeOffset UpdateTime { get; set; }

    public DateTimeOffset CheckTime { get; set; }
    
    public long NotificationTimes { get; set; }

    public bool ShouldCheckAgain => CheckTime < DateTimeOffset.UtcNow.AddMinutes(-2);

    public bool ShouldRemove => UpdateTime < DateTimeOffset.UtcNow.AddHours(3) && WorkflowRun.Conclusion == "success";
    
    public void ResetWorkflowRun(WorkflowRun workflowRun)
    {
        WorkflowRun = workflowRun;
        NotificationTimes = 0;
        UpdateTime = DateTimeOffset.UtcNow;
        CheckTime = DateTimeOffset.UtcNow;
    }

    public void AddNotificationTimes()
    {
        NotificationTimes += 1;
    }

    public void ResetCheckTime()
    {
        CheckTime = DateTimeOffset.UtcNow;
    }
    
    public bool ShouldNotice()
    {
        var firstNotification = NotificationTimes == 0 && WorkflowRun.IsFailure;
        var oneHourNotification = NotificationTimes == 1 
                                  && UpdateTime < DateTimeOffset.UtcNow.AddHours(-1) 
                                  && UpdateTime > DateTimeOffset.UtcNow.AddHours(-3)
                                  && WorkflowRun.IsFailure;
        var threeHourNotification = NotificationTimes == 2 
                                    && UpdateTime < DateTimeOffset.UtcNow.AddHours(-3) 
                                    && UpdateTime > DateTimeOffset.UtcNow.AddHours(-6)
                                    && WorkflowRun.IsFailure;
        var sixHourNotification = NotificationTimes == 3 
                                  && UpdateTime < DateTimeOffset.UtcNow.AddHours(-6) 
                                  && UpdateTime > DateTimeOffset.UtcNow.AddHours(-24)
                                  && WorkflowRun.IsFailure;
        var oneDayNotification = NotificationTimes == 4 
                                 && UpdateTime < DateTimeOffset.UtcNow.AddHours(-24)
                                 && WorkflowRun.IsFailure;
        return firstNotification 
               || oneHourNotification 
               || threeHourNotification 
               || sixHourNotification 
               || oneDayNotification;
    }
}