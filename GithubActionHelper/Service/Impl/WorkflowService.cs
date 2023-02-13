using GithubActionHelper.Client;

namespace GithubActionHelper.Service.Impl;

public class WorkflowService : IWorkflowService
{
    private readonly IGithubClient _githubClient;

    public WorkflowService(IGithubClient githubClient)
    {
        _githubClient = githubClient;
    }

    public async Task<List<Workflow>> FindActiveWorkflows(string owner, string repo)
    {
        var workflows = await _githubClient.GetWorkflows(owner, repo);
        return workflows.Where(item => item.State == "active").ToList();
    }

    public async Task<WorkflowRun> FindLastWorkflowRuns(string owner, string repo, long workflowId)
    {
        var workflowRuns = await _githubClient.GetWorkflowRun(owner, repo, workflowId);
        return workflowRuns
            .First();
    }

    public async Task<WorkflowRun> FindLastWorkflowRunsByBranch(string owner, string repo, long workflowId, string branch)
    {
        var workflowRuns = await _githubClient.GetWorkflowRun(owner, repo, workflowId, branch: branch);
        return workflowRuns
            .First();    
    }
}