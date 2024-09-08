using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public LogRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(Log log)
        {
            _applicationDbContext.Update(log);
        }
    }
}
