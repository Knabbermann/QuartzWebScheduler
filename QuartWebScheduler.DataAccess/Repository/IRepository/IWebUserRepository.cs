using HomePost8.Models;

namespace HomePost8.DataAccess.Repository.IRepository
{
    public interface IWebUserRepository : IRepository<WebUser>
    {
        void Update(WebUser applicationUser);
    }
}
