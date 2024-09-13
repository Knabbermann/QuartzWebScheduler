using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace QuartzWebScheduler.Utilities
{
    public class QuartzJob : IJob
    {
        private readonly string _connectionString;

        public QuartzJob()
        {
            if (_connectionString.IsNullOrEmpty())
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                _connectionString = configurationBuilder.GetConnectionString("WebDbContextConnection");
            }
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var result = "Test";

            var dataMap = context.JobDetail.JobDataMap;
            string jobConfigId = dataMap.GetString("Id");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = "INSERT INTO QuartzLogs (Id, Type, Date, Message, QuartzJobConfigId) VALUES (@Id, @Type, @Date, @Message, @QuartzJobConfigId)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                    command.Parameters.AddWithValue("@Type", "Information");
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@Message", result);
                    command.Parameters.AddWithValue("@QuartzJobConfigId", jobConfigId);

                    await command.ExecuteNonQueryAsync();
                }
            }

            await Task.CompletedTask;
        }
    }
}
