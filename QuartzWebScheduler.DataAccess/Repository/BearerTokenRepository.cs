using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class BearerTokenRepository : Repository<BearerToken>, IBearerTokenRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public BearerTokenRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(BearerToken bearerToken)
        {
            _applicationDbContext.Update(bearerToken);
        }
    }
}
