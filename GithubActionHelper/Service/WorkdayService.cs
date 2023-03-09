using GithubActionHelper.Service.Impl;

namespace GithubActionHelper.Service;

public class WorkdayService : IWorkdayService
{
    public bool IsWorkDay()
    {
        var now = DateTime.Now;
        return now.DayOfWeek >= DayOfWeek.Monday
               && now.DayOfWeek <= DayOfWeek.Friday
               && now.TimeOfDay >= new TimeSpan(9, 30, 0)
               && now.TimeOfDay <= new TimeSpan(18, 30, 0);
    }
}