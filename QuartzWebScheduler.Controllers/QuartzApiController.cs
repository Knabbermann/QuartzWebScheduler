using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuartzWebScheduler.Controllers.Interfaces;

namespace QuartzWebScheduler.Controllers
{
    [ApiController]
    [Route("api/quartz")]
    public class QuartzApiController : ControllerBase
    {
        private readonly IQuartzController _quartzController;

        public QuartzApiController(IQuartzController quartzController)
        {
            _quartzController = quartzController;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var status = _quartzController.GetSchedulerStatus();
            return Ok(status);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartAsync()
        {
            try
            {
                await _quartzController.StartSchedulerAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error while starting Quartz", details = ex.Message });
            }
        }

        [HttpPost("stop")]
        public async Task<IActionResult> StopAsync()
        {
            try
            {
                await _quartzController.StopSchedulerAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error while stopping Quartz", details = ex.Message });
            }
        }
    }

}
