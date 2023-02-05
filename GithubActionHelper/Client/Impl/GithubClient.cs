using GithubActionHelper.Service;

namespace GithubActionHelper.Client.Impl;

public class GithubClient : IGithubClient
{
    private readonly HttpClient _httpClient;
    
    public GithubClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<Workflow>> GetWorkflows(string owner, string repo)
    {
        var result = await GetAsync<WorkflowResponse>($"repos/{owner}/{repo}/actions/workflows");

        return result!.Workflows;
    }
    
    public async Task<List<WorkflowRun>> GetWorkflowRun(string owner, string repo, long workflowId, long pageSize = 1)
    {
        var result = await GetAsync<WorkflowRunResponse>($"repos/{owner}/{repo}/actions/workflows/{workflowId}/runs?per_page={pageSize}");

        return result!.WorkflowRuns;
    }
    
    private async Task<T?> GetAsync<T>(string path) where T : class
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, path);
        return await _httpClient.ExecuteAsync<T>(requestMessage);
    }
}