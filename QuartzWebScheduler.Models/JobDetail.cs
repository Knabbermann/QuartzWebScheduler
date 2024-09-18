using Quartz;

namespace QuartzWebScheduler.Models
{
    public class JobDetail
    {
        public JobKey JobKey { get; set; }

        public string CronExpression { get; set; }

        public DateTime? NextFireTime { get; set; }
    }
}
