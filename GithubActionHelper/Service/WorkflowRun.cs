using Newtonsoft.Json;

namespace GithubActionHelper.Service;

public class WorkflowRun
{
    public long Id { get; set; }
        
    public string Name { get; set; }
        
    [JsonProperty("head_branch")]
    public string Branch { get; set; }
        
    [JsonProperty("display_title")]
    public string Title { get; set; }
        
    public string Status { get; set; }
        
    public string Conclusion { get; set; }
        
    [JsonProperty("html_url")]
    public string Url { get; set; }
        
    [JsonProperty("created_at")]
    public DateTimeOffset CreatedTime { get; set; }
    
    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
        
    [JsonProperty("head_commit")]
    public Commit HeadCommit { get; set; }

    [JsonProperty("workflow_id")]
    public long WorkflowId { get; set; }
    
    public bool IsDependabot => Branch.StartsWith("dependabot");

    public bool IsCompleted => Status == "completed";

    public bool IsFailure => Conclusion == "failure";
    
    public bool IsSuccess => Conclusion == "success";
    
    public class Commit
    {
        public string Id { get; set; }
        
        public string Message { get; set; }
        
        public Author Author { get; set; }
    }

    public class Author
    {
        public string Name { get; set; }
        
        public string Email { get; set; }
    }

    public bool Equals(WorkflowRun other)
    {
        return Id.Equals(other.Id) && Conclusion.Equals(other.Conclusion);
    }
}
    
