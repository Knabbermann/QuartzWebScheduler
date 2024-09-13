﻿using Quartz.Impl;
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
                    .WithIdentity(jobConfig.JobName, "group1")
                    .UsingJobData("Url", jobConfig.Url)
                    .UsingJobData("Id", jobConfig.Id)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobConfig.JobName}_Trigger", "group1")
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

        public async Task StopSchedulerAsync()
        {
            await _scheduler.Shutdown();
        }

        private async Task<List<QuartzJobConfig>> GetQuartzJobConfigsAsync()
        {
            var jobConfigs = new List<QuartzJobConfig>();

            string query = "SELECT Id, JobName, CronExpression, Url FROM QuartzJobConfigs";

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
                                CronExpression = reader["CronExpression"].ToString(),
                                Url = reader["Url"].ToString()
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
