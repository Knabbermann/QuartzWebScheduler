using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using QuartzWebScheduler.Controllers.Interfaces;

namespace QuartzWebScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuartzStatusController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzStatusController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public IActionResult GetStatus()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var quartzController = scope.ServiceProvider.GetRequiredService<IQuartzController>();
                var status = quartzController.GetSchedulerStatus();
                return Ok(status);
            }
        }
    }
}
