using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface ISmtpConfigRepository : IRepository<SmtpConfig>
    {
        void Update(SmtpConfig smtpConfig);
    }
}
