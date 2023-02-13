using GithubActionHelper.Service;
using Newtonsoft.Json;

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
        Dictionary<string, NotificationRecord> repoNotificationRecords = 
            _githubSetting.Repos.ToDictionary(item => item.FullName, item => new NotificationRecord());
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
                var repoNotificationRecord = repoNotificationRecords[repo.FullName];
                _logger.LogInformation("[LastRun]: {Data}", JsonConvert.SerializeObject(lastRun));
                if (repoNotificationRecord.WorkflowRun != null && repoNotificationRecord.WorkflowRun.Id == lastRun.Id)
                {
                    if (lastRun.Conclusion == "failure" && repoNotificationRecord.ShouldNoticeAgain())
                    {
                        _logger.LogInformation("[LastRun] failure : {Data}", JsonConvert.SerializeObject(lastRun));
                        await SendFailedNotification(repoNotificationRecord, lastRun, repo.NickName);
                    }
                }
                else 
                {
                    repoNotificationRecord.ResetWorkflowRun(lastRun);
                    if (lastRun.Conclusion == "failure")
                    {
                        _logger.LogInformation("[LastRun] failure: {Data}", JsonConvert.SerializeObject(repoNotificationRecord));
                        await SendFailedNotification(repoNotificationRecord, lastRun, repo.NickName);
                    }
                }
            }
            _logger.LogInformation("Notification finished at: {Data}", DateTimeOffset.UtcNow.ToString());
            await Task.Delay(2*60*1000, stoppingToken);
        }
    }

    private async Task SendFailedNotification(NotificationRecord repoNotificationRecord, WorkflowRun lastRun, string repo)
    {
        repoNotificationRecord.AddNotificationTimes();
        var author = _githubSetting.Authors.Find(item => item.Email.ToLower() == lastRun.HeadCommit.Author.Email.ToLower());
        var notification = new Notification
        {
            CommitId = lastRun.HeadCommit.Id,
            CommitMessage = lastRun.HeadCommit.Message,
            Branch = lastRun.Branch,
            RunTime = lastRun.CreatedTime,
            Author = lastRun.HeadCommit.Author.Name,
            Mentioned = author != null ? author.Wechat : "@all",
            Repo = repo,
            Url = lastRun.Url,
            Name = lastRun.Name,
            Times = repoNotificationRecord.NotificationTimes
        };
        await _notificationService.SendNotification(notification);
    }
}
