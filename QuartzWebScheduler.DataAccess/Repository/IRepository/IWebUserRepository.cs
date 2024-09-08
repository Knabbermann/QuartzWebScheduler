using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface IWebUserRepository : IRepository<WebUser>
    {
        void Update(WebUser applicationUser);
    }
}
