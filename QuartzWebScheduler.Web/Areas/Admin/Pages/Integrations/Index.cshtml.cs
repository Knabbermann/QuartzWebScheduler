using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Utility;
using QuartzWebScheduler.Web.Pages;

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



        public void OnGet()
        {
        }
    }
}
