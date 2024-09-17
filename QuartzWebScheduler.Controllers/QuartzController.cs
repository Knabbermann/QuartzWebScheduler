using Quartz.Impl;
using Quartz;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuartzWebScheduler.Models;
using Microsoft.Data.SqlClient;
using Quartz.Impl.Matchers;

namespace QuartzWebScheduler.Controllers
{
    public class QuartzController : IQuartzController
    {
        private readonly IScheduler _scheduler;
        private readonly string _connectionString;

        public QuartzController()
        {
            if (_connectionString.IsNullOrEmpty())
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                _connectionString = configurationBuilder.GetConnectionString("WebDbContextConnection");
            }

            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        public async Task StartSchedulerAsync()
        {
            await _scheduler.Start();
            var jobConfigs = await GetQuartzJobConfigsAsync();
            foreach (var jobConfig in jobConfigs)
            {
                IJobDetail job = JobBuilder.Create<QuartzJob>()
                    .WithIdentity(jobConfig.JobName, jobConfig.GroupName)
                    .UsingJobData("RequestType", jobConfig.RequestType)
                    .UsingJobData("RequestUrl", jobConfig.RequestUrl)
                    .UsingJobData("RequestBody", jobConfig.RequestBody)
                    .UsingJobData("Id", jobConfig.Id)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobConfig.JobName}_Trigger", $"{jobConfig.GroupName}_Triggers")
                    .WithCronSchedule(jobConfig.CronExpression)
                    .Build();

                await _scheduler.ScheduleJob(job, trigger);
            }
        }

        public async Task TriggerJobByIdAsync(string id)
        {
            var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            foreach (var jobKey in jobKeys)
            {
                var detail = await _scheduler.GetJobDetail(jobKey);
                var jobDataMap = detail.JobDataMap;

                if (jobDataMap.ContainsKey("Id") && jobDataMap.GetString("Id") == id)
                {
                    await _scheduler.TriggerJob(jobKey);
                    return;
                }
            }

            Console.WriteLine($"Kein Job mit der Id {id} gefunden.");
        }

        public string GetSchedulerStatus()
        {
            if (_scheduler.IsShutdown) return "stopped";
            if (_scheduler.IsStarted) return "started";
            if (_scheduler.InStandbyMode) return "standby";
            return "stopped";
        }

        public async Task StopSchedulerAsync()
        {
            await _scheduler.Shutdown();
        }

        private async Task<List<QuartzJobConfig>> GetQuartzJobConfigsAsync()
        {
            var jobConfigs = new List<QuartzJobConfig>();

            string query = "SELECT Id, JobName, GroupName, CronExpression, RequestType, RequestUrl, RequestBody FROM QuartzJobConfigs WHERE IsActive = 1";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var jobConfig = new QuartzJobConfig
                            {
                                Id = reader["Id"].ToString(),
                                JobName = reader["JobName"].ToString(),
                                GroupName = reader["GroupName"].ToString(),
                                CronExpression = reader["CronExpression"].ToString(),
                                RequestType = reader["RequestType"].ToString(),
                                RequestUrl = reader["RequestUrl"].ToString(),
                                RequestBody = reader["RequestBody"].ToString()
                            };

                            jobConfigs.Add(jobConfig);
                        }
                    }
                }
            }

            return jobConfigs;
        }
    }
}
