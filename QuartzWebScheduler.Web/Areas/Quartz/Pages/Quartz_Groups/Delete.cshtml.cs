using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Groups
{
    public class DeleteModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public DeleteModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _logController = logController;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public QuartzGroup QuartzGroup { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Id))
            {
                _toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Quartz_Groups/Index");
            }

            QuartzGroup = _unitOfWork.QuartzGroup.GetFirstOrDefault(x => x.Id == Id);

            if (QuartzGroup == null)
            {
                _toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Quartz_Groups/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            QuartzGroup = _unitOfWork.QuartzGroup.GetSingleOrDefault(x => x.Id == Id);
            if (ModelState.IsValid)
            {
                _unitOfWork.QuartzGroup.Remove(QuartzGroup);
                _toastNotification.AddSuccessToastMessage("Successfully removed quartz group.");
                _logController.Log($"removed quartz group with id {Id}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                return RedirectToPage("/Quartz_Groups/Index");
            }

            return Page();
        }
    }
}
