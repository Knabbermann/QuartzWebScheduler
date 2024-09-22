using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using QuartzWebScheduler.Controllers;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Migrations;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class EditModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuartzController _quartzController;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public EditModel(IUnitOfWork unitOfWork,
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

        [BindProperty]
        public QuartzJobConfig QuartzJobConfig { get; set; }

        public List<SelectListItem> QuartzGroups { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Id))
            {
                _toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Quartz_Jobs/Index");
            }

            QuartzJobConfig = _unitOfWork.QuartzJobConfig.GetFirstOrDefault(x => x.Id == Id);

            if (QuartzJobConfig == null)
            {
                _toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Quartz_Jobs/Index");
            }

            QuartzGroups = _unitOfWork.QuartzGroup.GetAll().Select(x => new SelectListItem
            {
                Value = x.GroupName,
                Text = x.GroupName
            }).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!_quartzController.IsCronExpressionValid(QuartzJobConfig.CronExpression))
                ModelState.AddModelError("QuartzJobConfig.CronExpression", "Invalid CronExpression");

            if (ModelState.IsValid)
            {
                _unitOfWork.QuartzJobConfig.Update(QuartzJobConfig);
                _toastNotification.AddSuccessToastMessage("Successfully edited quartz job");
                _logController.Log($"edited quartz job with id {QuartzJobConfig.Id}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                return RedirectToPage("/Quartz_Jobs/Index");
            }

            return Page();
        }
    }
}
