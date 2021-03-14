using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AdventureWorks.DocFunctions
{
    public static class AdventureWorksDocumentsFunction
    {
        [FunctionName("AdventureWorksDocumentsFunction")]
        public static async void RunAsync([QueueTrigger("documentqueue", Connection = "StorageConnection")] string myQueueItem, ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            log.LogInformation(myQueueItem);

            //var connectionString = config["Entities"];
            var connectionString = "Server=tcp:advworks-sql-server.database.windows.net,1433;Initial Catalog=AdventureWorks;Persist Security Info=False;User ID=webapp;Password=xubsyf-fudpa0-faStoc;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var text = "INSERT INTO [Production].[Location] VALUES ('Zimbabwe', 0.0000, 0.00, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                }
            }
        }
    }
}
