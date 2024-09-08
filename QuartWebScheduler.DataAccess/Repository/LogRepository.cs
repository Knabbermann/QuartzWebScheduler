using HomePost8.DataAccess.DbContext;
using HomePost8.DataAccess.Repository.IRepository;
using HomePost8.Models;

namespace HomePost8.DataAccess.Repository
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
