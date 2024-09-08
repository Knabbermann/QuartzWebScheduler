using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class QuartzJobConfigRepository : Repository<QuartzJobConfig>, IQuartzJobConfigRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public QuartzJobConfigRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(QuartzJobConfig quartzJobConfig)
        {
            _applicationDbContext.Update(quartzJobConfig);
        }
    }
}
