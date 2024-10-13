using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Admin.Pages.Integrations
{
    [Authorize(Roles = StaticDetails.RoleAdmin)]
    public class IndexModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogController _logController;
        private readonly IToastNotification _toastNotification;

        public IndexModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _logController = logController;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public SmtpConfig SmtpConfig { get; set; }

        [BindProperty]
        public TeamsConfig TeamsConfig { get; set; }

        public IActionResult OnGet()
        {
            SmtpConfig = _unitOfWork.SmtpConfig.GetAll().FirstOrDefault();
            TeamsConfig = _unitOfWork.TeamsConfig.GetAll().FirstOrDefault();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (SmtpConfig.IsActive)
            {
                if(SmtpConfig.Id is null)
                {
                    SmtpConfig.Id = new Guid().ToString();
                    _unitOfWork.SmtpConfig.Add(SmtpConfig);
                    _toastNotification.AddSuccessToastMessage("Successfully added smtp config");
                    _logController.Log($"added smtp config", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    return RedirectToPage("/Index");
                }
                else if (ModelState.IsValid)
                {
                    _unitOfWork.SmtpConfig.Update(SmtpConfig);
                    _toastNotification.AddSuccessToastMessage("Successfully edited smtp config");
                    _logController.Log($"edited smtp config", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    return RedirectToPage("/Index");
                }
            }

            if (TeamsConfig.IsActive)
            {
                if (TeamsConfig.Id is null)
                {
                    TeamsConfig.Id = new Guid().ToString();
                    _unitOfWork.TeamsConfig.Add(TeamsConfig);
                    _toastNotification.AddSuccessToastMessage("Successfully added teams config");
                    _logController.Log($"added teams config", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    return RedirectToPage("/Index");
                }
                else if (ModelState.IsValid)
                {
                    _unitOfWork.TeamsConfig.Update(TeamsConfig);
                    _toastNotification.AddSuccessToastMessage("Successfully edited teams config");
                    _logController.Log($"edited teams config", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    return RedirectToPage("/Index");
                }
            }

            return Page();
        }
    }
}
