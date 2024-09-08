using HomePost8.DataAccess.DbContext;
using HomePost8.DataAccess.Repository.IRepository;
using HomePost8.Models;

namespace HomePost8.DataAccess.Repository
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
