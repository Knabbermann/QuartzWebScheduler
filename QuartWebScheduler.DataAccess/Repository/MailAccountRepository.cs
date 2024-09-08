using HomePost8.DataAccess.DbContext;
using HomePost8.DataAccess.Repository.IRepository;
using HomePost8.Models;

namespace HomePost8.DataAccess.Repository
{
    public class MailAccountRepository : Repository<MailAccount>, IMailAccountRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public MailAccountRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(MailAccount mailAccount)
        {
            _applicationDbContext.Update(mailAccount);
        }
    }
}
