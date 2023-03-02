namespace GithubActionHelper.Service.Impl;

public class WorkFlowRunContainer : IWorkFlowRunContainer
{
    private readonly Dictionary<string, WorkflowRun> _container;

    public WorkFlowRunContainer()
    {
        _container = new Dictionary<string, WorkflowRun>();
    }

    public void Push(string key, WorkflowRun workflowRun)
    {
        if (workflowRun.IsCompleted)
        {
            
        }
    }
}