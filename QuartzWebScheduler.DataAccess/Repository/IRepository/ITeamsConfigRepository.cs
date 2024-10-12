using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface ITeamsConfigRepository : IRepository<TeamsConfig>
    {
        void Update(TeamsConfig teamsConfig);
    }
}
