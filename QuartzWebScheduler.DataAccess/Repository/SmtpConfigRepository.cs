using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class SmtpConfigRepository : Repository<SmtpConfig>, ISmtpConfigRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public SmtpConfigRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(SmtpConfig smtpConfig)
        {
            _applicationDbContext.Update(smtpConfig);
        }
    }
}
