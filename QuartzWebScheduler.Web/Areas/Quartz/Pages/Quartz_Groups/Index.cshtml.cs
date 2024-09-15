using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Groups
{
    [Authorize]
    public class IndexModel : CustomPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork,
            IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<QuartzGroup> QuartzGroups { get; set; }

        public void OnGet()
        {
            QuartzGroups = _unitOfWork.QuartzGroup.GetAll();
        }
    }
}
