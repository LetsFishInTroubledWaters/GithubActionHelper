namespace GithubActionHelper.Service;

public class WorkflowRunRecord : WorkflowRun
{
    public WorkflowRunRecord(WorkflowRun workflowRun, DateTimeOffset nextNotificationTime)
    {
        Id = workflowRun.Id;
        Name = workflowRun.Name;
        Branch = workflowRun.Branch;
        Title = workflowRun.Title;
        Status = workflowRun.Status;
        Conclusion = workflowRun.Conclusion;
        Url = workflowRun.Url;
        CreatedTime = workflowRun.CreatedTime;
        UpdatedAt = workflowRun.UpdatedAt;
        HeadCommit = workflowRun.HeadCommit;
        WorkflowId = workflowRun.WorkflowId;
        NextNextNotificationTime = nextNotificationTime;
        NotificationInterval = new Queue<int>();
        NotificationInterval.Enqueue(1);
        NotificationInterval.Enqueue(3);
        NotificationInterval.Enqueue(6);
        NotificationInterval.Enqueue(24);
    }

    public DateTimeOffset? NextNextNotificationTime { get; set; }

    public Queue<int> NotificationInterval { get; }

    public int Times { get; set; }
}
    
