namespace GithubActionHelper.Service;

public class GithubSetting
{
    public GithubSetting(string repoString, string authorString, string owner)
    {
        Repos = repoString.Split(",")
            .Select(item =>
            {
                var repoInfo = item.Split(":");
                return new Repo
                {
                    FullName = repoInfo.First().Trim(),
                    NickName = repoInfo.Last().Trim()
                };
            })
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
    public List<Repo> Repos { get; set; }
    
    public string Owner { get; set; }
    
    public List<Author> Authors { get; set; }

    public class Author
    {
        public string Email { get; set; }
        
        public string Wechat { get; set; }
    }

    public class Repo
    {
        public string FullName { get; set; }

        public string NickName { get; set; }
    }
}