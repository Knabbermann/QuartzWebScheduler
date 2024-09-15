using QuartzWebScheduler.Models;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface IQuartzGroupRepository : IRepository<QuartzGroup>
    {
        public void Update(QuartzGroup quartzGroup);
    }
}
