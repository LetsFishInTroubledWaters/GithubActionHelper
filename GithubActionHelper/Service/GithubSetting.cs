namespace GithubActionHelper.Service;

public class GithubSetting
{
    public List<string> Repos { get; set; }
    
    public string Owner { get; set; }
    
    public List<Author> Authors { get; set; }

    public class Author
    {
        public string Name { get; set; }
        
        public string Wechat { get; set; }
    }
}