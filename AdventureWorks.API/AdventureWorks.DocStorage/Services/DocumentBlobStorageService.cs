using System;
using System.Threading.Tasks;
using AdventureWorks.DocStorage.Interfaces;
using AdventureWorks.DocStorage.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AdventureWorks.DocStorage.Services
{
    public class DocumentBlobStorageService : IDocumentStorageService
    {
        private const string STORAGE_ACCOUNT_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=advworksstorage;AccountKey=YXLMoq5Ie6ubryNjJxc9SrOV/9e8qT70VrPpzBafC6tdqzAPNm7JS8DcXi0rG1c6KKanmSZP8goGtf2Iaa8kxA==;EndpointSuffix=core.windows.net";
        private const string BLOB_CONTAINER_NAME = "adv-wrks-documents";


        public async Task<Uri> AddDocumentAsync(WordDocumentModel document)
        {
            var blobServiceClient = new BlobServiceClient(STORAGE_ACCOUNT_CONNECTION_STRING);

            var blobContainerClient = blobServiceClient.GetBlobContainerClient(BLOB_CONTAINER_NAME);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = blobContainerClient.GetBlobClient(document.FileName);
            await blobClient.UploadAsync(document.FileContent, new BlobHttpHeaders { ContentType = document.ContentType });

            return blobClient.Uri;
        }
    }
}
