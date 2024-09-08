using QuartzWebScheduler.Utility;

namespace QuartzWebScheduler.Controllers.Interfaces
{
    public interface ILogController
    {
        public List<Models.Log> GetAllLogs();
        public void Log(string message, string userId, string? type = StaticDetails.LogTypeInformation);
    }
}
