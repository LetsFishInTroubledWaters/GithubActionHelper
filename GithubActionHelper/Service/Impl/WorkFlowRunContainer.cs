namespace GithubActionHelper.Service.Impl;

public class WorkFlowRunContainer : IWorkFlowRunContainer
{
    private readonly Dictionary<string, WorkflowRunRecord> _container;

    public WorkFlowRunContainer()
    {
        _container = new Dictionary<string, WorkflowRunRecord>();
    }

    public void Add(string key, WorkflowRun workflowRun)
    {
        if (!workflowRun.IsCompleted || workflowRun.IsDependabot) return;
        
        var record = new WorkflowRunRecord(workflowRun, DateTimeOffset.UtcNow);
        if (!_container.ContainsKey(key) || workflowRun.IsSuccess || !_container[key].Equals(record))
        {
            _container[key] = record;
        }
    }

    public List<KeyValuePair<string, WorkflowRunRecord>> GetRecordNeedToNotify()
    {
        var now = DateTimeOffset.UtcNow;
        var recordNeedToNotify = _container.Where(pair => pair.Value.IsFailure)
            .Where(pair => pair.Value.NextNextNotificationTime != null)
            .Where(pair => now > pair.Value.NextNextNotificationTime)
            .ToList();
        foreach (var record in recordNeedToNotify.Select(keyValuePair => keyValuePair.Value))
        {
            if (record.NotificationInterval.Count == 0)
            {
                record.NextNextNotificationTime = null;
            }
            else
            {
                record.NextNextNotificationTime =
                    record.NextNextNotificationTime!.Value.AddHours(record.NotificationInterval.Dequeue());
                record.Times += 1;
            }
        }

        return recordNeedToNotify;
    }
}