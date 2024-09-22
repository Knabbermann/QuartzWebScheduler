using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Quartz;
using QuartzWebScheduler.Controllers;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class DeleteModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuartzController _quartzController;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public DeleteModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _quartzController = quartzController;
            _toastNotification = toastNotification;
            _logController = logController;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public QuartzJobConfig QuartzJobConfig { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Id))
            {
                _toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Quartz_Jobs/Index");
            }
            QuartzJobConfig = _unitOfWork.QuartzJobConfig.GetSingleOrDefault(x => x.Id == Id);

            return Page();
        }

        public IActionResult OnPost()
        {
            QuartzJobConfig = _unitOfWork.QuartzJobConfig.GetSingleOrDefault(x => x.Id == Id);
            if(QuartzJobConfig == null)
            {
                _toastNotification.AddErrorToastMessage("quartzJobConfig is null");
                return RedirectToPage("/Quartz_Jobs/Index");
            }

            _unitOfWork.QuartzJobConfig.Remove(QuartzJobConfig);
            _unitOfWork.SaveChanges();
            _quartzController.DeleteJobByKey(new JobKey(QuartzJobConfig.JobName,QuartzJobConfig.GroupName));
            _toastNotification.AddSuccessToastMessage("Successfully deleted quartz job");
            _logController.Log($"deleted quartz job with id {QuartzJobConfig.Id}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return RedirectToPage("/Quartz_Jobs/Index");
            
        }
    }
}
