using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using QuartzWebScheduler.Controllers;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class CreateModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public CreateModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _logController = logController;
        }

        [BindProperty]
        public QuartzJobConfig QuartzJobConfig { get; set; }


        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.QuartzJobConfig.Add(QuartzJobConfig);
                _toastNotification.AddSuccessToastMessage("Successfully created quartz job.");
                _logController.Log($"created new quartz job with name {QuartzJobConfig.JobName}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                return RedirectToPage("/Quartz_Jobs/Index");
            }

            return Page();
        }
    }
}
