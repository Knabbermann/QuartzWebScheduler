using Quartz.Impl;
using Quartz;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.Utilities;

namespace QuartzWebScheduler.Controllers
{
    public class QuartzController : IQuartzController
    {
        private readonly IScheduler _scheduler;

        public QuartzController()
        {
            // Scheduler erstellen
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        // Startet den Scheduler und plant einen Job
        public async Task StartSchedulerAsync()
        {
            await _scheduler.Start();

            // Definiere den Job
            IJobDetail job = JobBuilder.Create<QuartzJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Definiere den Trigger (alle 5 Sekunden ausführen)
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();

            // Job mit dem Trigger dem Scheduler hinzufügen
            await _scheduler.ScheduleJob(job, trigger);
        }

        // Stoppt den Scheduler
        public async Task StopSchedulerAsync()
        {
            await _scheduler.Shutdown();
        }
    }
}
