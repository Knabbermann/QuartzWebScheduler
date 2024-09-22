using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using QuartzWebScheduler.Web.Pages;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Areas.Admin.Pages.Auths
{
    [Authorize(Roles = StaticDetails.RoleAdmin)]
    public class BearerModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogController _logController;
        private readonly IToastNotification _toastNotification;

        public BearerModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            _logController = logController;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public BearerToken BearerToken { get; set; }

        public IActionResult OnGet()
        {
            BearerToken = _unitOfWork.BearerToken.GetAll().FirstOrDefault();
            return Page();
        }

        public IActionResult OnPost()
        {
            if(BearerToken.Id is null && BearerToken.Token is not null)
            {
                BearerToken.Id = new Guid().ToString();
                _unitOfWork.BearerToken.Add(BearerToken);
                _toastNotification.AddSuccessToastMessage("Successfully added bearer token");
                _logController.Log($"added bearer token", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                return RedirectToPage("/Index");
            }

            else if (ModelState.IsValid)
            {
                _unitOfWork.BearerToken.Update(BearerToken);
                _toastNotification.AddSuccessToastMessage("Successfully edited bearer token");
                _logController.Log($"edited bearer token", userId: HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
