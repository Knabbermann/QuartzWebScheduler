using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;

namespace QuartzWebScheduler.Web.Pages
{
    public class IndexModel : CustomPageModel
    {

        public IndexModel(IQuartzController quartzController, 
            IToastNotification toastNotification, 
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
        }

        public void OnGet()
        {

        }
    }
}
