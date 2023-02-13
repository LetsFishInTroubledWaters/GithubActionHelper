using GithubActionHelper.Service;

namespace GithubActionHelper.Client;

public interface IGithubClient
{
    Task<List<Workflow>> GetWorkflows(string owner, string repo);

    Task<List<WorkflowRun>> GetWorkflowRun(string owner, string repo, long workflowId, long pageSize = 1, string? branch = null);
}