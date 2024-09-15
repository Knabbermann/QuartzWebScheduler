﻿using System.Data;

namespace QuartzWebScheduler.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IWebUserRepository WebUser { get; }
        ILogRepository Log { get; }
        IQuartzJobConfigRepository QuartzJobConfig { get; }
        IQuartzLogRepository QuartzLog { get; }
        void SaveChanges();
        IDbConnection GetDbConnection();
    }
}
