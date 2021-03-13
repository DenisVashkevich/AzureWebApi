using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


namespace AdventureWorks.DocFunctions
{
    public static class AdventureWorksDocumentsFunction
    {
        [FunctionName("AdventureWorksDocumentsFunction")]
        public static void Run([QueueTrigger("adv-wrks-documents-notification")] string myQueueItem, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"Queue trigger function processed: {myQueueItem}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config["Entities"];


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SELECT TOP(1000) [DocumentNode]
      ,[DocumentLevel]
      ,[Title]
      ,[Owner]
      ,[FolderFlag]
      ,[FileName]
      ,[FileExtension]
      ,[Revision]
      ,[ChangeNumber]
      ,[Status]
      ,[DocumentSummary]
      ,[Document]
      ,[rowguid]
      ,[ModifiedDate]
                FROM[Production].[Document]

                var query = @"INSERT INTO Production.Document(name) values(@name)";

                using (var command = new SqlCommand(query, conn))
                {

                }
            }
        }
    }
}
