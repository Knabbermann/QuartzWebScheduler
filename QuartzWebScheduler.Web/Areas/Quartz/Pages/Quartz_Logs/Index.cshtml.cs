using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Logs
{
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
        public IEnumerable<QuartzLog> QuartzLogs { get; set; }

        public void OnGet()
        {
            QuartzLogs = _unitOfWork.QuartzLog.GetAll(includeProperties: "QuartzJobConfig").OrderByDescending(x => x.Date);
        }
    }
}