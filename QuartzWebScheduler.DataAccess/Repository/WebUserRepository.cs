using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class WebUserRepository : Repository<WebUser>, IWebUserRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public WebUserRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(WebUser webUser)
        {
            _applicationDbContext.Update(webUser);
        }
    }
}
