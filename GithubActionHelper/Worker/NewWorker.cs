using GithubActionHelper.Proxy;
using GithubActionHelper.Service;

namespace GithubActionHelper.Worker;

public class NewWorker : BackgroundService
{
    private readonly IWorkflowService _workflowService;

    private readonly GithubSetting _githubSetting;

    private readonly IWorkFlowRunContainer _container;

    public NewWorker(GithubSetting githubSetting, IWorkflowService workflowService, IWorkFlowRunContainer container)
    {
        _githubSetting = githubSetting;
        _workflowService = workflowService;
        _container = DynamicProxy<IWorkFlowRunContainer>.Create(container);
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
                _container.Push(key, lastRun);
            }

            await Task.Delay(2 * 60 * 1000, stoppingToken);
        }
    }
}