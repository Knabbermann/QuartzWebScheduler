namespace QuartzWebScheduler.Controllers.Interfaces
{
    public interface IQuartzController
    {
        public Task StartSchedulerAsync();
        public Task TriggerJobByIdAsync(string id);
        public string GetSchedulerStatus();
        public Task StopSchedulerAsync();
    }
}
