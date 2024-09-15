using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.Utility;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Pages
{
    public class CustomPageModel : PageModel
    {
        private readonly IQuartzController _quartzController;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public CustomPageModel(IQuartzController quartzController, 
            IToastNotification toastNotification, 
            ILogController logController)
        {
            _quartzController = quartzController;
            _toastNotification = toastNotification;
            _logController = logController;
        }

        [Authorize]
        public async Task<IActionResult> OnPostStartQuartzServiceAsync()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToPage();
            if (_quartzController.GetSchedulerStatus().Equals("started")) return RedirectToPage();

            try
            {
                await _quartzController.StartSchedulerAsync();
                _toastNotification.AddSuccessToastMessage("Successfully started quartz.");
                _logController.Log("Started quartz successfully", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error while starting quartz: {ex.Message}");
                _logController.Log($"Error while starting quartz: {ex.Message}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), StaticDetails.LogTypeError);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostStopQuartzServiceAsync()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToPage();
            try
            {
                await _quartzController.StopSchedulerAsync();
                _toastNotification.AddSuccessToastMessage("Successfully stopped quartz.");
                _logController.Log("Stopped quartz successfully", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error while stopping quartz: {ex.Message}");
                _logController.Log($"Error while stopping quartz: {ex.Message}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), StaticDetails.LogTypeError);
            }
            return RedirectToPage();
        }
    }
}
