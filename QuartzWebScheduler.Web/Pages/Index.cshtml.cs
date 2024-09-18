using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.Web.Pages
{
    public class IndexModel : CustomPageModel
    {
        private readonly IQuartzController _quartzController;

        public IndexModel(IQuartzController quartzController, 
            IToastNotification toastNotification, 
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _quartzController = quartzController;
        }

        [BindProperty(SupportsGet = true)]
        public List<JobDetail> JobDetails { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            JobDetails = await _quartzController.GetJobDetailsAsync();
            return Page();
        }
    }
}
