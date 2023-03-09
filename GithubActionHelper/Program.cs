using System.Net.Http.Headers;
using GithubActionHelper.Client;
using GithubActionHelper.Client.Impl;
using GithubActionHelper.Service;
using GithubActionHelper.Service.Impl;
using GithubActionHelper.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        HttpSetting httpSetting = configuration.GetSection("Http").Get<HttpSetting>();
        var repoString = configuration.GetSection("Github:Repos").Get<string>();
        var authorString = configuration.GetSection("Github:Authors").Get<string>();
        var ownerString = configuration.GetSection("Github:Owner").Get<string>();
        var githubSettingSetting = new GithubSetting(repoString, authorString, ownerString);
        services.AddSingleton(httpSetting);
        services.AddSingleton(githubSettingSetting);
        services.AddSingleton<IWorkflowService, WorkflowService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IWorkFlowRunContainer, WorkFlowRunContainer>();
        services.AddSingleton<IWorkdayService, WorkdayService>();

        services.AddHttpClient<IGithubClient, GithubClient>(client =>
            {
                var header = new AuthenticationHeaderValue("Bearer", httpSetting.Github.Token);
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("GithubActionHelper");
                client.DefaultRequestHeaders.Authorization = header;
                client.BaseAddress = new Uri(httpSetting.Github.Url);
                client.Timeout = TimeSpan.FromSeconds(60);
            });
        services.AddHttpClient<IWechatClient, WechatClient>(client =>
            {
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("GithubActionHelper");
                client.BaseAddress = new Uri(httpSetting.Wechat.Url);
                client.Timeout = TimeSpan.FromSeconds(60);
            });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();