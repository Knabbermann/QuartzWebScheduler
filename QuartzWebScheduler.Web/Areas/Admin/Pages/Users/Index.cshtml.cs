using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;
using QuartzWebScheduler.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuartzWebScheduler.Web.Areas.Admin.Pages.Users
{
    [Authorize(Roles = StaticDetails.RoleAdmin)]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork,
            UserManager<WebUser> userManager)
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
