using Quartz.Impl;
using Quartz;
using QuartzWebScheduler.Controllers.Interfaces;
using QuartzWebScheduler.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuartzWebScheduler.Models;
using Microsoft.Data.SqlClient;

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
            // Scheduler erstellen
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        // Startet den Scheduler und plant einen Job
        public async Task StartSchedulerAsync()
        {
            await _scheduler.Start();
            var jobConfigs = await GetQuartzJobConfigsAsync();
            foreach (var jobConfig in jobConfigs)
            {
                // Erstelle den Job
                IJobDetail job = JobBuilder.Create<QuartzJob>() // Du kannst hier den eigentlichen Job-Typ einsetzen
                    .WithIdentity(jobConfig.JobName, "group1")
                    .UsingJobData("Url", jobConfig.Url) // Übergabe von Daten an den Job
                    .Build();

                // Erstelle den Trigger basierend auf der CronExpression aus der Konfiguration
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobConfig.JobName}_Trigger", "group1")
                    .WithCronSchedule(jobConfig.CronExpression) // Verwendet die CronExpression aus der Config
                    .Build();

                // Füge den Job mit dem Trigger dem Scheduler hinzu
                await _scheduler.ScheduleJob(job, trigger);
            }
        }


        // Stoppt den Scheduler
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
