using System.Data;

namespace HomePost8.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IWebUserRepository WebUser { get; }
        ILogRepository Log { get; }
        IMailAccountRepository MailAccount { get; }
        void SaveChanges();
        IDbConnection GetDbConnection();
    }
}
