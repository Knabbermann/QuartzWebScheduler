using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using QuartzWebScheduler.Controllers;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;
using Quartz;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class CreateModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuartzController _quartzController;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public CreateModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _quartzController = quartzController;
            _toastNotification = toastNotification;
            _logController = logController;
        }

        [BindProperty]
        public QuartzJobConfig QuartzJobConfig { get; set; }

        public List<SelectListItem> QuartzGroups { get; set; }

        public void OnGet()
        {
            QuartzGroups = _unitOfWork.QuartzGroup.GetAll().Select(x => new SelectListItem
            {
                Value = x.GroupName,
                Text = x.GroupName
            }).ToList();
        }

        public IActionResult OnPost()
        {
            if(!_quartzController.IsCronExpressionValid(QuartzJobConfig.CronExpression))
                ModelState.AddModelError("QuartzJobConfig.CronExpression", "Invalid CronExpression");

            if (ModelState.IsValid)
            {
                _unitOfWork.QuartzJobConfig.Add(QuartzJobConfig);
                _unitOfWork.SaveChanges();
                if(QuartzJobConfig.IsActive) _quartzController.LoadJob(QuartzJobConfig);
                _toastNotification.AddSuccessToastMessage("Successfully created quartz job.");
                _logController.Log($"created new quartz job with name {QuartzJobConfig.JobName}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                return RedirectToPage("/Quartz_Jobs/Index");
            }

            return Page();
        }
    }
}
