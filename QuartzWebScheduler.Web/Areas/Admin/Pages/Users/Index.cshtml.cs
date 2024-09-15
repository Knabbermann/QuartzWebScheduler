using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using QuartzWebScheduler.Web.Pages;

namespace QuartzWebScheduler.Web.Areas.Admin.Pages.Users
{
    [Authorize(Roles = StaticDetails.RoleAdmin)]
    public class IndexModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork,
            UserManager<WebUser> userManager,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
            UserManager = userManager;
        }

        public IEnumerable<WebUser> Users { get; set; }
        public UserManager<WebUser> UserManager { get; }

        public void OnGet()
        {
            Users = _unitOfWork.WebUser.GetAll();
        }
    }
}
