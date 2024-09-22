using Quartz;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.Controllers.Interfaces
{
    public interface IQuartzController
    {
        bool IsCronExpressionValid(string cronExpression);
        public Task StartSchedulerAsync();
        public Task TriggerJobByIdAsync(string id);
        public Task TriggerJobByKeyAsync(JobKey jobKey);
        public Task<IReadOnlyCollection<IJobExecutionContext>> GetAllExecutingJobsAsync();
        public string GetSchedulerStatus();
        public Task<List<JobDetail>> GetJobDetailsAsync();
        public Task StopSchedulerAsync();
    }
}
