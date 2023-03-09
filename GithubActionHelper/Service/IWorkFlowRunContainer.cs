namespace GithubActionHelper.Service;

public interface IWorkFlowRunContainer
{
    void Add(string key, WorkflowRun workflowRun);
    List<KeyValuePair<string, WorkflowRunRecord>> GetRecordNeedToNotify();
}