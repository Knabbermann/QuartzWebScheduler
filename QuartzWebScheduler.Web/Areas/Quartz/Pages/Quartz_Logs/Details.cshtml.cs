using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Logs
{
    [Authorize]
    public class DetailsModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;

        public DetailsModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public QuartzLog QuartzLog { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Id))
            {
                _toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Quartz_Logs/Index");
            }

            QuartzLog = _unitOfWork.QuartzLog.GetFirstOrDefault(x => x.Id == Id, includeProperties: "QuartzJobConfig");
            if (QuartzLog == null)
            {
                _toastNotification.AddErrorToastMessage("QuartzJobConfig is null");
                return RedirectToPage("/Quartz_Logs/Index");
            }

            return Page();
        }
    }
}
