using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface IBearerTokenRepository : IRepository<BearerToken>
    {
        public void Update(BearerToken bearerToken);
    }
}
