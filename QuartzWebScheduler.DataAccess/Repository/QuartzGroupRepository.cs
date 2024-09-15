using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class QuartzGroupRepository : Repository<QuartzGroup>, IQuartzGroupRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public QuartzGroupRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(QuartzGroup quartzGroup)
        {
            _applicationDbContext.Update(quartzGroup);
        }
    }
}
