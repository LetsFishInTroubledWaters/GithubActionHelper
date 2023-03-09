using GithubActionHelper.Service;

namespace GithubActionHelper.Worker;

public class NewWorker : BackgroundService
{
    private readonly IWorkflowService _workflowService;

    private readonly GithubSetting _githubSetting;

    private readonly IWorkFlowRunContainer _container;
    
    private readonly INotificationService _notificationService;

    public NewWorker(GithubSetting githubSetting, IWorkflowService workflowService, IWorkFlowRunContainer container, INotificationService notificationService)
    {
        _githubSetting = githubSetting;
        _workflowService = workflowService;
        _container = container;
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var repo in _githubSetting.Repos)
            {
                var workflows = await _workflowService.FindActiveWorkflows(_githubSetting.Owner, repo.FullName);
                var workflowRuns = await Task.WhenAll(workflows.Select(workflow =>
                        _workflowService.FindLastWorkflowRuns(_githubSetting.Owner, repo.FullName, workflow.Id)).ToList());
                var lastRun = workflowRuns.OrderByDescending(item => item.CreatedTime).First();
                var key = $"{repo.NickName}/{lastRun.Branch}";
                _container.Add(key, lastRun);
            }
            _ = HandleFailedData();

            await Task.Delay(2 * 60 * 1000, stoppingToken);
        }
    }

    private async Task HandleFailedData()
    {
        var notifications = _container.GetRecordNeedToNotify()
            .Select(pair =>
            {
                var author = _githubSetting.Authors.Find(item =>
                    item.Email.ToLower() == pair.Value.HeadCommit.Author.Email.ToLower());
                return new Notification
                {
                    CommitId = pair.Value.HeadCommit.Id,
                    CommitMessage = pair.Value.HeadCommit.Message,
                    Branch = pair.Value.Branch,
                    RunTime = pair.Value.CreatedTime,
                    Author = pair.Value.HeadCommit.Author.Name,
                    Mentioned = author != null ? author.Wechat : "@all",
                    Repo = pair.Key.Split("/")[0],
                    Url = pair.Value.Url,
                    Name = pair.Value.Name,
                    Times = pair.Value.Times
                };
            }).ToList();
        await _notificationService.SendNotification(notifications);
    }
}