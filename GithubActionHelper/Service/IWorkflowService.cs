namespace GithubActionHelper.Service;

public interface IWorkflowService
{
    Task<List<Workflow>> FindActiveWorkflows(string owner, string repo);

    Task<WorkflowRun> FindLastWorkflowRuns(string owner, string repo, long workflowId);
    
    Task<WorkflowRun> FindLastWorkflowRunsByBranch(string owner, string repo, long workflowId, string branch);
}