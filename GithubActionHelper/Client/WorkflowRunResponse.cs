using GithubActionHelper.Service;
using Newtonsoft.Json;

namespace GithubActionHelper.Client;

public class WorkflowRunResponse
{
    [JsonProperty("total_count")]
    public long Count { get; set; }
    
    [JsonProperty("workflow_runs")]
    public List<WorkflowRun> WorkflowRuns { get; set; }
}