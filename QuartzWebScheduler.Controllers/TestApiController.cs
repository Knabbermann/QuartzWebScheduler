using Microsoft.AspNetCore.Mvc;

namespace QuartzWebScheduler.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestApiController : ControllerBase
    {
        [HttpGet("getEndpoint")]
        public async Task<IActionResult> GetStatus()
        {
            var cDelay = new Random().Next(0, 11);
            await Task.Delay(TimeSpan.FromSeconds(cDelay));

            var isSuccess = cDelay < 7;
            if (isSuccess) return Ok("Get successful");
            return StatusCode(500, "An error occurred during GET.");
        }

        [HttpPost("postEndpoint")]
        public async Task<IActionResult> PostStatus([FromBody] object? payload = null)
        {
            var cDelay = new Random().Next(0, 11);
            await Task.Delay(TimeSpan.FromSeconds(cDelay));

            var isSuccess = cDelay < 7;
            if (isSuccess) return Ok("Post successful");
            return StatusCode(500, "An error occurred during POST.");
        }

        [HttpPut("putEndpoint")]
        public async Task<IActionResult> PutStatus([FromBody] object? payload = null)
        {
            var cDelay = new Random().Next(0, 11);
            await Task.Delay(TimeSpan.FromSeconds(cDelay));

            var isSuccess = cDelay < 7;
            if (isSuccess) return Ok("Put successful");
            return StatusCode(500, "An error occurred during PUT.");
        }

        [HttpPatch("patchEndpoint")]
        public async Task<IActionResult> PatchStatus([FromBody] object? payload = null)
        {
            var cDelay = new Random().Next(0, 11);
            await Task.Delay(TimeSpan.FromSeconds(cDelay));

            var isSuccess = cDelay < 7;
            if (isSuccess) return Ok("Patch successful");
            return StatusCode(500, "An error occurred during PATCH.");
        }

        [HttpDelete("deleteEndpoint/{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var cDelay = new Random().Next(0, 11);
            await Task.Delay(TimeSpan.FromSeconds(cDelay));

            var isSuccess = cDelay < 7;
            if (isSuccess) return Ok($"Delete successful for ID: {id}");
            return StatusCode(500, "An error occurred during DELETE.");
        }
    }
}
