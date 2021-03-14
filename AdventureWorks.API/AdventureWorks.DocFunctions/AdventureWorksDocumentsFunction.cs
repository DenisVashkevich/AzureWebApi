using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using AdventureWorks.DocFunctions.Models;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.IO;
using System;
using System.Net;
using System.Text;

namespace AdventureWorks.DocFunctions
{
    public static class AdventureWorksDocumentsFunction
    {
        [FunctionName("AdventureWorksDocumentsFunction")]
        public static async void RunAsync([QueueTrigger("documentqueue", Connection = "StorageConnection")] string myQueueItem, ILogger log, ExecutionContext context)
        {
            //log.LogInformation($"Queue trigger function processed: {myQueueItem}");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            log.LogInformation(myQueueItem);

            var docmeta = JsonSerializer.Deserialize<DocumentMetadaSerializationModel>(Encoding.UTF8.GetString(Convert.FromBase64String(myQueueItem)));

            log.LogInformation(docmeta.Title);

            var webclient = new WebClient();

            var data = webclient.OpenRead(docmeta.DocumentUrl);
            using (var reader = new BinaryReader(data))
            {
                var connectionString = config["Entities"];

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

                    //command.Connection = connection;
                    //command.Transaction = transaction;
                    var compiler = new SqlServerCompiler();

                    //string query = "INSERT INTO [Production].[Location] VALUES (@EmployeeID, @EmployeeName, @DocumentName, @FileName, @FileContentType, @FileData)";
                    string query = "INSERT INTO [Production].[Location] VALUES('Zimbabwe', '0.0000', '0.00', '2008-04-30 00:00:00.000')";
                    command.CommandText = query;
                    await command.ExecuteNonQueryAsync();
                    await command.Transaction.CommitAsync();
                    await connection.CloseAsync();

                    //using (SqlCommand cmd = new SqlCommand(query))
                    //{
                    //    cmd.Connection = connection;
                    //    cmd.Parameters.AddWithValue("@EmployeeID", ddlEmployees.SelectedItem.Value);
                    //    cmd.Parameters.AddWithValue("@EmployeeName", ddlEmployees.SelectedItem.Text);
                    //    cmd.Parameters.AddWithValue("@DocumentName", txtDocument.Text);
                    //    cmd.Parameters.AddWithValue("@FileName", empFilename);
                    //    cmd.Parameters.AddWithValue("@FileContentType", FilecontentType);
                    //    cmd.Parameters.AddWithValue("@FileData", Databytes);
                    //    //open database connection  
                    //    connection.Open();
                    //    //execute SQL statement   
                    //    cmd.ExecuteNonQuery();
                    //    connection.Close();
                    //}

                    //var db = new QueryFactory(connection, compiler);

                    try
                    {

                        //var query = db.Query("[Production].[Document]").Insert(new
                        //{
                        //    Title = docmeta.Title,
                        //    Owner = 217,
                        //    FolderFlag = 0,
                        //    FileName = docmeta.FileName,
                        //    FileExtension = docmeta.FileExtension,
                        //    Revision = docmeta.Revision,
                        //    ChangeNumber = docmeta.ChangeNuber,
                        //    Status = docmeta.Status,
                        //    DocumentSummary = docmeta.DocumentSummary,
                        //    Document = reader.ReadBytes((int)data.Length)

                        //}).ToString();

                        log.LogInformation(query);

                        //command.CommandText = $@"INSERT INTO [Production].[Document]([DocumentNode]
                        //                                          ,[DocumentLevel]
                        //                                          ,[Title]
                        //                                          ,[Owner]
                        //                                          ,[FolderFlag]
                        //                                          ,[FileName]
                        //                                          ,[FileExtension]
                        //                                          ,[Revision]
                        //                                          ,[ChangeNumber]
                        //                                          ,[Status]
                        //                                          ,[DocumentSummary]
                        //                                          ,[Document]
                        //                                          ,[rowguid]
                        //                                          ,[ModifiedDate]) values(@name)";

                        //command.CommandText = query;
                        //await command.ExecuteNonQueryAsync();
                        //await command.Transaction.CommitAsync();
                    }
                    catch(Exception e)
                    {
                        log.LogError(e.ToString());
                    }

                }
            }
        }
    }
}
