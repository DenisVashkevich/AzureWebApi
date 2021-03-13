using System;
using System.IO;
using System.Threading.Tasks;
using AdventureWorks.DocStorage.Interfaces;
using AdventureWorks.DocStorage.Models;
using AdventureWorks.DocStorage.Utils;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;

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

        public async Task<string> AddDocumentAsync(WordDocumentModel document)
        {
            var blobServiceClient = new BlobServiceClient(_configuration[Defines.STORAGE_ACCOUNT_CONNECTION_STRING_SECTTION]);

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_configuration[Defines.BLOB_CONTAINER_NAME_SECTION]);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = blobContainerClient.GetBlobClient(document.FileName);
            var uploadresult = await blobClient.UploadAsync(document.FileContent, new BlobHttpHeaders { ContentType = document.ContentType });

            if(uploadresult.GetRawResponse().Status == StatusCodes.Status201Created)
            {
                var documentMetadata = new DocumentMetadaSerializationModel()
                {
                    Title = document.Title,
                    Owner = 210,
                    FolderFlag = 0,
                    FileName = Path.GetFileNameWithoutExtension(document.FileName),
                    FileExtension = Path.GetExtension(document.FileName),
                    Revision = 2,
                    ChangeNuber = 3,
                    Status = 4,
                    DocumentSummary = document.Summary,
                    DocumentUrl = blobClient.Uri.AbsoluteUri,
                    ModifiedDate = DateTime.Today.ToString()
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                var base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(documentMetadata, options)));
                await _uploadNotificationService.NotifyOnUploadAsync(base64Message);

                return blobClient.Uri.AbsoluteUri;
            }

            return string.Empty;
        }
    }
}