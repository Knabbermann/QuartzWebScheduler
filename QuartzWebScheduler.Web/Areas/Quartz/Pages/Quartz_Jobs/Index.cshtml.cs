using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.Web.Areas.Quartz.Pages.Quartz_Jobs
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<QuartzJobConfig> QuartzJobConfigs { get; set; }

        public void OnGet()
        {
            QuartzJobConfigs = _unitOfWork.QuartzJobConfig.GetAll();
        }
    }
}
