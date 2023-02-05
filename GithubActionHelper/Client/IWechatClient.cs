namespace GithubActionHelper.Client;

public interface IWechatClient
{
    public Task<T?> PostMessageAsync<T>(object body) where T : class;
}