namespace QuartzWebScheduler.Controllers.Interfaces
{
    public interface IQuartzController
    {
        public Task StartSchedulerAsync();
        public Task StopSchedulerAsync();
    }
}
