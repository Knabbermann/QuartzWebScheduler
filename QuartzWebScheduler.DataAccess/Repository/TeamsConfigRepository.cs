using QuartzWebScheduler.DataAccess.DbContext;
using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository
{
    public class TeamsConfigRepository : Repository<TeamsConfig>, ITeamsConfigRepository
    {
        private readonly WebDbContext _applicationDbContext;

        public TeamsConfigRepository(WebDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Update(TeamsConfig teamsConfig)
        {
            _applicationDbContext.Update(teamsConfig);
        }
    }
}
