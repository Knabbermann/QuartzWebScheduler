using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Text;

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
            var dataMap = context.JobDetail.JobDataMap;
            string jobConfigId = dataMap.GetString("Id");
            string requestType = dataMap.GetString("RequestType");
            string requestUrl = dataMap.GetString("RequestUrl");
            string requestBody = dataMap.GetString("RequestBody");

            string result = string.Empty;
            int statusCode = 0;

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;

                if (requestType.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(requestUrl, content);
                }
                else if (requestType.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    response = await client.GetAsync(requestUrl);
                }
                else if (requestType.Equals("PUT", StringComparison.OrdinalIgnoreCase))
                {
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    response = await client.PutAsync(requestUrl, content);
                }
                else if (requestType.Equals("PATCH", StringComparison.OrdinalIgnoreCase))
                {
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUrl) { Content = content };
                    response = await client.SendAsync(request);
                }
                else if (requestType.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
                {
                    response = await client.DeleteAsync(requestUrl);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported request type: {requestType}");
                }

                statusCode = (int)response.StatusCode;
                result = await response.Content.ReadAsStringAsync();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = "INSERT INTO QuartzLogs (Id, Type, Date, Message, QuartzJobConfigId, StatusCode) " +
                             "VALUES (@Id, @Type, @Date, @Message, @QuartzJobConfigId, @StatusCode)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                    command.Parameters.AddWithValue("@Type", "Information");
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@Message", result);
                    command.Parameters.AddWithValue("@QuartzJobConfigId", jobConfigId);
                    command.Parameters.AddWithValue("@StatusCode", statusCode);

                    await command.ExecuteNonQueryAsync();
                }
            }

            await Task.CompletedTask;
        }

    }
}
