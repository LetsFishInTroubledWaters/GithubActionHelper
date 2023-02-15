using GithubActionHelper.Service;

namespace GithubActionHelper.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IWorkflowService _workflowService;

    private readonly INotificationService _notificationService;

    private readonly GithubSetting _githubSetting;

    public Worker(ILogger<Worker> logger, GithubSetting githubSetting, IWorkflowService workflowService, INotificationService notificationService)
    {
        _logger = logger;
        _githubSetting = githubSetting;
        _workflowService = workflowService;
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<NotificationRecord> repoNotificationRecords = new List<NotificationRecord>();
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var repo in _githubSetting.Repos)
            {
                var result = await _workflowService.FindActiveWorkflows(_githubSetting.Owner, repo.FullName);
                var runs = new List<WorkflowRun>();
                foreach (var workflow in result)
                {
                    var run = await _workflowService.FindLastWorkflowRuns(_githubSetting.Owner, repo.FullName, workflow.Id);
                    runs.Add(run);
                }

                var lastRun = runs.OrderByDescending(item => item.CreatedTime).First();
                await HandleNotification(lastRun, repoNotificationRecords, repo);
            }

            repoNotificationRecords.RemoveAll(item => item.ShouldRemove);
            var notificationRecords = repoNotificationRecords.FindAll(item => item.ShouldCheckAgain).ToList();
            foreach (var notificationRecord in notificationRecords)
            {
                var run = await _workflowService.FindLastWorkflowRunsByBranch(
                    _githubSetting.Owner, 
                    notificationRecord.Repo.FullName, notificationRecord.WorkflowRun.WorkflowId, 
                    notificationRecord.Branch);
                await HandleNotification(run, repoNotificationRecords, notificationRecord.Repo);
            }
            _logger.LogInformation("Notification finished at: {Data}", DateTimeOffset.UtcNow.ToString());
            await Task.Delay(2*60*1000, stoppingToken);
        }
    }

    private async Task HandleNotification(WorkflowRun lastRun, List<NotificationRecord> repoNotificationRecords, GithubSetting.Repo repo)
    {
        if (lastRun.IsDependabot)
        {
            return;
        }

        var repoNotificationRecord = repoNotificationRecords
            .Find(item =>
                item.Branch == lastRun.Branch
                && item.Repo.FullName == repo.FullName);
        if (repoNotificationRecord == null)
        {
            repoNotificationRecord = new NotificationRecord(lastRun, repo);
            repoNotificationRecords.Add(repoNotificationRecord);
        }
        else if (!repoNotificationRecord.WorkflowRun.IsCompleted
                 || repoNotificationRecord.WorkflowRun.Id != lastRun.Id
                 || repoNotificationRecord.WorkflowRun.Conclusion != lastRun.Conclusion)
        {
            repoNotificationRecord.ResetWorkflowRun(lastRun);
        }

        if (repoNotificationRecord.ShouldNotice())
        {
            await SendFailedNotification(repoNotificationRecord, repo.NickName);
        }
        repoNotificationRecord.ResetCheckTime();
    }

    private async Task SendFailedNotification(NotificationRecord repoNotificationRecord, string repo)
    {
        repoNotificationRecord.AddNotificationTimes();
        var workFlowRun = repoNotificationRecord.WorkflowRun;
        var author = _githubSetting.Authors.Find(item => item.Email.ToLower() == workFlowRun.HeadCommit.Author.Email.ToLower());
        var notification = new Notification
        {
            CommitId = workFlowRun.HeadCommit.Id,
            CommitMessage = workFlowRun.HeadCommit.Message,
            Branch = workFlowRun.Branch,
            RunTime = workFlowRun.CreatedTime,
            Author = workFlowRun.HeadCommit.Author.Name,
            Mentioned = author != null ? author.Wechat : "@all",
            Repo = repo,
            Url = workFlowRun.Url,
            Name = workFlowRun.Name,
            Times = repoNotificationRecord.NotificationTimes
        };
        await _notificationService.SendNotification(notification);
    }
}
