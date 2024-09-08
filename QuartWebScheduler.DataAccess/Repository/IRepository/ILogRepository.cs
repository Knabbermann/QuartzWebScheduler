using HomePost8.Models;

namespace HomePost8.DataAccess.Repository.IRepository
{
    public interface ILogRepository : IRepository<Log>
    {
        void Update(Log log);
    }
}
