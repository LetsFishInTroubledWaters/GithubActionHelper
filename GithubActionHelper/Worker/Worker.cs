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
            _githubSetting.Repos.ToDictionary(item => item, item => new NotificationRecord());
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var repo in _githubSetting.Repos)
            {
                var result = await _workflowService.FindActiveWorkflows(_githubSetting.Owner, repo);
                var runs = new List<WorkflowRun>();
                foreach (var workflow in result)
                {
                    var run = await _workflowService.FindLastWorkflowRuns(_githubSetting.Owner, repo, workflow.Id);
                    runs.Add(run);
                }

                var lastRun = runs.OrderByDescending(item => item.CreatedTime).First();
                var repoNotificationRecord = repoNotificationRecords[repo];
                if (repoNotificationRecord.WorkflowRun != null && repoNotificationRecord.WorkflowRun.Id == lastRun.Id)
                {
                    
                }
                else if(lastRun.Conclusion == "failure")
                {
                    var author = _githubSetting.Authors.Find(item => item.Name == lastRun.HeadCommit.Author.Email);
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
                        Name = lastRun.Name
                    };
                    await _notificationService.SendNotification(notification);
                    repoNotificationRecord.ResetWorkflowRun(lastRun);
                }

                var data = JsonConvert.SerializeObject(result);
                _logger.LogInformation("Worker running at: {Data}", data);
            }
            await Task.Delay(5*60*1000, stoppingToken);
        }
    }
}
