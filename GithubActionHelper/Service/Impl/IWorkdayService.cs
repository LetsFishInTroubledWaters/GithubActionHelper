namespace GithubActionHelper.Service.Impl;

public interface IWorkdayService
{
    // 暂时只以星期&事件判断工作时间，后续可以修改为从网络上获取节假日表
    bool IsWorkDay();
}