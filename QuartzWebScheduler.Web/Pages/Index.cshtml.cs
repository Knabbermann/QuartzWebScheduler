using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Quartz;
using QuartzWebScheduler.Controllers;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using System.Security.Claims;

namespace QuartzWebScheduler.Web.Pages
{
    public class IndexModel : CustomPageModel
    {
        private readonly IQuartzController _quartzController;
        private readonly ILogController _logController;
        private readonly IToastNotification _toastNotification;

        public IndexModel(IQuartzController quartzController, 
            IToastNotification toastNotification, 
            ILogController logController) : base(quartzController, toastNotification, logController)
        {
            _quartzController = quartzController;
            _logController = logController;
            _toastNotification = toastNotification;
        }

        [BindProperty(SupportsGet = true)]
        public List<JobDetail> JobDetails { get; set; }

        [BindProperty(SupportsGet = true)]
        public IReadOnlyCollection<IJobExecutionContext> ExecutingJobs { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            JobDetails = await _quartzController.GetJobDetailsAsync();
            ExecutingJobs = await _quartzController.GetAllExecutingJobsAsync();
            return Page();
        }
    }
}
