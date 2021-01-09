using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AdventureWorks.DocFunctions
{
    public static class AdventureWorksDocumentsFunction
    {
        [FunctionName("AdventureWorksDocumentsFunction")]
        public static void Run([QueueTrigger("adv-wrks-documents-notification", Connection = "AdvWorksProductsApi:Storage:ConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
