using QuartzWebScheduler.Controllers.Interfaces;

namespace QuartzWebScheduler.Web
{
    public class QuartzBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var quartzController = scope.ServiceProvider.GetRequiredService<IQuartzController>();
                await quartzController.StartSchedulerAsync();
            }
        }
    }
}
