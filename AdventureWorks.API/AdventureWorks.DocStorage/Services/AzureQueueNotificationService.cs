using System.Threading.Tasks;
using AdventureWorks.DocStorage.Interfaces;
using AdventureWorks.DocStorage.Utils;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;

namespace AdventureWorks.DocStorage.Services
{
    public class AzureQueueNotificationService: IUploadNotificationService
    {
        private readonly IConfiguration _configuration;

        public AzureQueueNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task NotifyOnUploadAsync(string message)
        {
            var queueClient = new QueueClient(_configuration[Defines.STORAGE_ACCOUNT_CONNECTION_STRING_SECTTION], _configuration[Defines.QUEUE_NAME_SECTION]);
            queueClient.CreateIfNotExists();

            await queueClient.SendMessageAsync(message);
        }
    }
}