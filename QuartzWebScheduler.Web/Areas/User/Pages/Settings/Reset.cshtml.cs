using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.User.Pages.Settings
{
    [Authorize]
    public class ResetModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<WebUser> _userManager;
        private readonly IToastNotification _toastNotification;

        public ResetModel(IUnitOfWork unitOfWork,
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

        [BindProperty]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (cUserId == null)
            {
                _toastNotification.AddErrorToastMessage("can not find NameIdentifier for current user");
                RedirectToPage("/Settings/Index");
            }

            User = _unitOfWork.WebUser.GetSingleOrDefault(x => x.Id.Equals(cUserId));

            if (User == null)
            {
                _toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Settings/Index");
            }

            if (ModelState.IsValid)
            {
                await _userManager.RemovePasswordAsync(User);
                await _userManager.AddPasswordAsync(User, Password);
                _toastNotification.AddSuccessToastMessage("Successfully changed password");
                return RedirectToPage("/Settings/Index");
            }

            return Page();
        }
    }
}
