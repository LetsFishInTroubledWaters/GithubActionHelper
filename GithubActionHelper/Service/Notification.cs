using System.Diagnostics;

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
    
    public long Times { get; set; }

    public string NotificationTitle => TitleMap.ContainsKey(Times) ? TitleMap[Times] : "Workflow构建失败";

    private Dictionary<long, string> TitleMap =
        new()
        {
            { 1, "构建失败" },
            { 2, "构建失败-已超过1小时" },
            { 3, "构建失败-已超过3小时" },
            { 4, "构建失败-已超过6小时" },
            { 5, "构建失败-已超过24小时" }
        };
}