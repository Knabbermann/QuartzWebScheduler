using HomePost8.DataAccess.DbContext;
using HomePost8.DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HomePost8.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebDbContext _webDbContext;
        private readonly IConfiguration _configuration;

        public UnitOfWork(WebDbContext webDbContext, IConfiguration configuration)
        {
            _webDbContext = webDbContext;
            _configuration = configuration;
            WebUser = new WebUserRepository(_webDbContext);
            Log = new LogRepository(_webDbContext);
            MailAccount = new MailAccountRepository(_webDbContext);
        }

        public IWebUserRepository WebUser { get; }
        public ILogRepository Log { get; }

        public IMailAccountRepository MailAccount { get; }

        public void SaveChanges()
        {
            _webDbContext.SaveChanges();
        }

        public IDbConnection GetDbConnection()
        {
            var connectionString = _configuration.GetConnectionString("WebDbContextConnection");
            return new SqlConnection(connectionString);
        }
    }
}
