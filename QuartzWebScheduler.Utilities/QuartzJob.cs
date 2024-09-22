using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using QuartzWebScheduler.Utility;
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
            bool UsingAuth = dataMap.GetBooleanValue("UsingAuth");

            string result = string.Empty;
            int statusCode = 0;
            var type = StaticDetails.LogTypeInformation;
            string bearerToken = "";

            if (UsingAuth)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = "SELECT TOP 1 Token FROM BearerTokens ORDER BY Id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        var token = await command.ExecuteScalarAsync();

                        if (token != null)
                        {
                            bearerToken = token.ToString();
                        }
                    }
                }
            }

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;

                if(UsingAuth) 
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

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
                
                if(!response.IsSuccessStatusCode) type = StaticDetails.LogTypeError;

                result = await response.Content.ReadAsStringAsync();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = "INSERT INTO QuartzLogs (Id, Type, Date, Message, QuartzJobConfigId, StatusCode, CronExpression, RequestType, RequestUrl, RequestBody) " +
                             "VALUES (@Id, @Type, @Date, @Message, @QuartzJobConfigId, @StatusCode, @CronExpression, @RequestType, @RequestUrl, @RequestBody)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                    command.Parameters.AddWithValue("@Type", type.ToString());
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@Message", result);
                    command.Parameters.AddWithValue("@QuartzJobConfigId", jobConfigId);
                    command.Parameters.AddWithValue("@StatusCode", statusCode);
                    command.Parameters.AddWithValue("@CronExpression", GetCronExpressionFromContext(context));
                    command.Parameters.AddWithValue("@RequestType", requestType);
                    command.Parameters.AddWithValue("@RequestUrl", requestUrl);
                    command.Parameters.AddWithValue("@RequestBody", requestBody);

                    await command.ExecuteNonQueryAsync();
                }
            }

            await Task.CompletedTask;
        }

        private string GetCronExpressionFromContext(IJobExecutionContext context)
        {
            var trigger = context.Trigger;

            if (trigger is ICronTrigger cronTrigger) return cronTrigger.CronExpressionString;

            return "Failed to Load.";
        }

    }
}
