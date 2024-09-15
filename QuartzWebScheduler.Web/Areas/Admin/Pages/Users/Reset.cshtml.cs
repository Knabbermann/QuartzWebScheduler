using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Admin.Pages.Users
{
    [Authorize]
    public class ResetModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<WebUser> _userManager;
        private readonly IToastNotification _toastNotification;
        private readonly ILogController _logController;

        public ResetModel(IUnitOfWork unitOfWork,
            UserManager<WebUser> userManager,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _logController = logController;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

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
            if (string.IsNullOrEmpty(Id))
            {
                _toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Users/Index");
            }

            User = _unitOfWork.WebUser.GetFirstOrDefault(x => x.Id == Id);

            if (User == null)
            {
                _toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Users/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                _toastNotification.AddErrorToastMessage("Id is null");
                return RedirectToPage("/Users/Index");
            }

            User = _unitOfWork.WebUser.GetFirstOrDefault(x => x.Id == Id);

            if (User == null)
            {
                _toastNotification.AddErrorToastMessage("Object is null");
                return RedirectToPage("/Users/Index");
            }

            if (ModelState.IsValid)
            {
                await _userManager.RemovePasswordAsync(User);
                await _userManager.AddPasswordAsync(User, Password);
                _toastNotification.AddSuccessToastMessage("Successfully changed password");
                _logController.Log($"reset user password with id {User.Id}", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                return RedirectToPage("/Users/Index");
            }

            return Page();
        }
    }
}
