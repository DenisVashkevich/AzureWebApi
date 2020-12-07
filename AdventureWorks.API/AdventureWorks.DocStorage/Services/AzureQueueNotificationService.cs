using System.Threading.Tasks;
using AdventureWorks.DocStorage.Interfaces;
using Azure.Storage.Queues;


namespace AdventureWorks.DocStorage.Services
{
    public class AzureQueueNotificationService: IUploadNotificationService
    {
        private const string STORAGE_ACCOUNT_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=advworksstorage;AccountKey=YXLMoq5Ie6ubryNjJxc9SrOV/9e8qT70VrPpzBafC6tdqzAPNm7JS8DcXi0rG1c6KKanmSZP8goGtf2Iaa8kxA==;EndpointSuffix=core.windows.net";
        private const string QUEUE_NAME = "adv-wrks-documents-notification";
        private const string MESSAGE_TEMPLATE = "New file uploaded:";

        public async Task NotifyOnUploadAsync(string fileName)
        {
            var queueClient = new QueueClient(STORAGE_ACCOUNT_CONNECTION_STRING, QUEUE_NAME);
            queueClient.CreateIfNotExists();

            await queueClient.SendMessageAsync(MESSAGE_TEMPLATE + $" {fileName}");
        }
    }
}
