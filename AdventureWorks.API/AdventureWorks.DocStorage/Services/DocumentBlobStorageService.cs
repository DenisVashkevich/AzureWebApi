using System;
using System.Threading.Tasks;
using AdventureWorks.DocStorage.Interfaces;
using AdventureWorks.DocStorage.Models;
using AdventureWorks.DocStorage.Utils;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace AdventureWorks.DocStorage.Services
{
    public class DocumentBlobStorageService : IDocumentStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly IUploadNotificationService _uploadNotificationService;

        public DocumentBlobStorageService(IUploadNotificationService uploadNotificationService, IConfiguration configuration)
        {
            _uploadNotificationService = uploadNotificationService;
            _configuration = configuration;
        }

        public async Task<Uri> AddDocumentAsync(WordDocumentModel document)
        {
            var blobServiceClient = new BlobServiceClient(_configuration[Defines.STORAGE_ACCOUNT_CONNECTION_STRING_SECTTION]);

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_configuration[Defines.BLOB_CONTAINER_NAME_SECTION]);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = blobContainerClient.GetBlobClient(document.FileName);
            await blobClient.UploadAsync(document.FileContent, new BlobHttpHeaders { ContentType = document.ContentType });

            await _uploadNotificationService.NotifyOnUploadAsync(document.FileName);

            return blobClient.Uri;
        }
    }
}