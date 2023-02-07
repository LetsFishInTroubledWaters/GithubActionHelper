namespace GithubActionHelper.Service;

public class Notification
{
    public string Repo { get; set; }
    
    public string Mentioned { get; set; }
    
    public DateTimeOffset RunTime { get; set; }
    
    public string CommitMessage { get; set; }
    
    public string CommitId { get; set; }
    
    public string Author { get; set; }
    
    public string Branch { get; set; }
    
    public string Url { get; set; }
    
    public string Name { get; set; }
}