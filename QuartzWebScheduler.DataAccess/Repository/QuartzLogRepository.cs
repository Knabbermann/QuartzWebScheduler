using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class QuartzLogRepository : Repository<QuartzLog>, IQuartzLogRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public QuartzLogRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
