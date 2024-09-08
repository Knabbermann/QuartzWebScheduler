using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface ILogRepository : IRepository<Log>
    {
        void Update(Log log);
    }
}
