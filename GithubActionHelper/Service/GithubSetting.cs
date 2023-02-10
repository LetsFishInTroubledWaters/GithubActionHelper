namespace GithubActionHelper.Service;

public class GithubSetting
{
    public GithubSetting(string repoString, string authorString, string owner)
    {
        Repos = repoString.Split(",")
            .Select(item => item.Trim())
            .ToList();
        Authors = authorString.Split(",")
            .Select(item =>
            {
                var authorInfo = item.Split(":");
                return new Author
                {
                    Email = authorInfo.First().Trim(),
                    Wechat = authorInfo.Last().Trim()
                };
            }).ToList();
        Owner = owner;
    }
    public List<string> Repos { get; set; }
    
    public string Owner { get; set; }
    
    public List<Author> Authors { get; set; }

    public class Author
    {
        public string Email { get; set; }
        
        public string Wechat { get; set; }
    }
}