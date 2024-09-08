using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface IQuartzJobConfigRepository : IRepository<QuartzJobConfig>
    {
        void Update(QuartzJobConfig quartzJobConfig);
    }
}
