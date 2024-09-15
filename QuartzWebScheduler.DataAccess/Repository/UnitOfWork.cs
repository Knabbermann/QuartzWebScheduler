using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace QuartzWebScheduler.DataAccess.Repository
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
            QuartzJobConfig = new QuartzJobConfigRepository(_webDbContext);
            QuartzLog = new QuartzLogRepository(_webDbContext);
            QuartzGroup = new QuartzGroupRepository(_webDbContext);
        }

        public IWebUserRepository WebUser { get; }
        public ILogRepository Log { get; }
        public IQuartzJobConfigRepository QuartzJobConfig { get; }
        public IQuartzLogRepository QuartzLog {  get; }
        public IQuartzGroupRepository QuartzGroup { get; }

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
