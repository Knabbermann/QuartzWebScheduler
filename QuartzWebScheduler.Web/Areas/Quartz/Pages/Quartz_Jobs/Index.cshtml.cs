using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;
        private readonly IServiceProvider _serviceProvider;

        public IndexModel(IUnitOfWork unitOfWork, 
            IToastNotification toastNotification, 
            ILogController logController,
            IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _logController = logController;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<QuartzJobConfig> QuartzJobConfigs { get; set; }

        public void OnGet()
        {
            QuartzJobConfigs = _unitOfWork.QuartzJobConfig.GetAll();
        }

        public async Task<IActionResult> OnPostStartAsync(string id)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var quartzController = scope.ServiceProvider.GetRequiredService<IQuartzController>();
                    await quartzController.TriggerJobByIdAsync(id);
                    _toastNotification.AddSuccessToastMessage($"{id} triggered successfully");
                    _logController.Log($"successfull triggered QuartzJob:'{id} manually'", HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Error while triggering QuartzJob {id} manually: {ex}");
                _logController.Log($"failed triggering QuartzJob {id} manually: {ex}", HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), StaticDetails.LogTypeError);
            }
            return RedirectToPage();
        }
    }
}
