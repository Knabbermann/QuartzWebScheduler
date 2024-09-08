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
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public DeleteModel(IUnitOfWork unitOfWork,
            IToastNotification toastNotification,
            ILogController logController)
        {
            _unitOfWork = unitOfWork;
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
            _toastNotification.AddSuccessToastMessage("Successfully deleted quartz job");
            _logController.Log($"deleted quartz job with id {QuartzJobConfig.Id}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return RedirectToPage("/Quartz_Jobs/Index");
            
        }
    }
}
