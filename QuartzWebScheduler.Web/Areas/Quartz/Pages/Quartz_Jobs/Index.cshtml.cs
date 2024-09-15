using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class IndexModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuartzController _quartzController;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;
        private readonly IServiceProvider _serviceProvider;

        public IndexModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController,
            IServiceProvider serviceProvider) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _quartzController = quartzController;
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
                await _quartzController.TriggerJobByIdAsync(id);
                _toastNotification.AddSuccessToastMessage($"{id} triggered successfully");
                _logController.Log($"successfull triggered QuartzJob:'{id} manually'", HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
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
