﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet("executingJobs")]
        public async Task<IActionResult> GetExecutingJobs()
        {
            var executingJobs = await _quartzController.GetAllExecutingJobsAsync();

            var jobList = executingJobs.Select(job => new
            {
                job.JobDetail.Key.Group,
                job.JobDetail.Key.Name,
                job.ScheduledFireTimeUtc,
                job.FireTimeUtc
            });

            return new JsonResult(jobList);
        }

        [HttpGet("jobDetails")]
        public async Task<IActionResult> GetJobDetails()
        {
            return new JsonResult(await _quartzController.GetJobDetailsAsync());
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
