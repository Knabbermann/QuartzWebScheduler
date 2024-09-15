using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Web.Pages;

namespace QuartzWebScheduler.Web.Areas.User.Pages.Logs
{
    public class IndexModel : CustomPageModel
    {
        private readonly ILogController _logController;

        public IndexModel(IQuartzController quartzController,
            IToastNotification toastNotification,
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _logController = logController;
        }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Log> Logs { get; set; }

        public void OnGet()
        {
            Logs = _logController.GetAllLogs().OrderByDescending(x => x.CreatedDate);
        }
    }
}
