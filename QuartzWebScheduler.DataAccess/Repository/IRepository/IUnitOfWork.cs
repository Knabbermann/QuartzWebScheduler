using System.Data;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IWebUserRepository WebUser { get; }
        ILogRepository Log { get; }
        void SaveChanges();
        IDbConnection GetDbConnection();
    }
}
