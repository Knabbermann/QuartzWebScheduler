using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public EditModel(IUnitOfWork unitOfWork,
            IToastNotification toastNotification,
            ILogController logController)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _logController = logController;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public QuartzJobConfig QuartzJobConfig { get; set; }

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

            return Page();
        }

        public IActionResult OnPost()
        {
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
