﻿using QuartzWebScheduler.DataAccess.Repository.IRepository;
using QuartzWebScheduler.Utility;
using QuartzWebScheduler.Controllers.Interfaces;

namespace QuartzWebScheduler.Controllers
{
    public class LogController : ILogController
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Models.Log> GetAllLogs()
        {
            return _unitOfWork.Log.GetAll(includeProperties:"WebUser").ToList();
        }

        public void Log(string message, string userId, string? type = StaticDetails.LogTypeInformation)
        {
            var cUser = _unitOfWork.WebUser.GetSingleOrDefault(x => x.Id.Equals(userId));
            var cLog = new Models.Log
            {
                Message = message,
                Type = type,
                WebUserId = cUser.Id,
                WebUser = cUser
            };
            _unitOfWork.Log.Add(cLog);
            _unitOfWork.SaveChanges();
        }
    }
}
