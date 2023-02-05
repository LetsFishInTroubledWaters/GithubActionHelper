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
}