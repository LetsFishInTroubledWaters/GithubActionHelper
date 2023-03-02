namespace GithubActionHelper.Service;

public interface IWorkFlowRunContainer
{
    void Push(string key, WorkflowRun workflowRun);
}