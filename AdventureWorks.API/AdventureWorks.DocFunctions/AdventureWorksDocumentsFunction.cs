using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text;
using AdventureWorks.DocFunctions.Models;
using System;

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

            var docmeta = JsonSerializer.Deserialize<DocumentMetadaSerializationModel>(Encoding.UTF8.GetString(Convert.FromBase64String(myQueueItem)));

            WebClient webclient = new WebClient();
            var dataStream = webclient.OpenRead(docmeta.DocumentUrl);

            using (var reader = new BinaryReader(dataStream))
            {
                var data = reader.ReadBytes((int)dataStream.Length);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    //var text = "INSERT INTO [Production].[Document] VALUES ('Zimbabwe', 0.0000, 0.00, GETDATE())";
                    var query = $@"INSERT INTO [Production].[Document]([DocumentNode],
                                                                        Title,
                                                                        Owner,
                                                                        FileName,
                                                                        FileExtension,
                                                                        Revision,
                                                                        Status,
                                                                        DocumentSummary,
                                                                        Document]) values(VALUES(CAST('/' AS hierarchyid).GetDescendant(NULL, NULL), {docmeta.Title}, 210, {docmeta.FileName}, {docmeta.FileExtension}, {docmeta.Revision}, {docmeta.Status}, {docmeta.DocumentSummary}, {data})";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        var rows = await cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"{rows} rows were updated");
                    }
                }
            }
        }
    }
}
