using GithubActionHelper.Service;
using Newtonsoft.Json;

namespace GithubActionHelper.Client;

public class WorkflowResponse
{
    [JsonProperty("total_count")]
    public long Count { get; set; }
    
    public List<Workflow> Workflows { get; set; }
}