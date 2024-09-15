using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using QuartzWebScheduler.Web.Pages;
using QuartzWebScheduler.Controllers.Interfaces;

namespace QuartzWebScheduler.Web.Areas.User.Pages.Settings
{
    [Authorize]
    public class IndexModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<WebUser> _userManager;
        private readonly IToastNotification _toastNotification;

        public IndexModel(IUnitOfWork unitOfWork,
            UserManager<WebUser> userManager,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        public WebUser User { get; set; }
        public string UserRole { get; set; }

        public IActionResult OnGet()
        {
            var cUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (cUserId == null)
            {
                _toastNotification.AddErrorToastMessage("can not find NameIdentifier for current user");
                RedirectToPage("/Index");
            }

            User = _unitOfWork.WebUser.GetSingleOrDefault(x => x.Id.Equals(cUserId));
            if (User == null)
            {
                _toastNotification.AddErrorToastMessage("can not find current user in database");
                RedirectToPage("/Index");
            }
            UserRole = _userManager.GetRolesAsync(User).Result.FirstOrDefault();

            return Page();
        }
    }
}
