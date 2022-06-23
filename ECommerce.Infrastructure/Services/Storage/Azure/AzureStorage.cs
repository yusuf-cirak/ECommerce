using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ECommerce.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage:Storage,IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient; 
        BlobContainerClient _blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            List<(string fileName, string path)> datas = new();
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName); // Azure'da container'lar aslında klasörlerimiz olduğu için önce hangi klasörde çalışmamız gerektiğini öğrenmemiz gerekiyor. Bunu da ContainerClient nesnesi üzerinde değişiklik yaparak yapacağız.

            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            foreach (IFormFile file in files)
            {
                string fileNewName=await FileRenameAsync(containerName, file.Name,HasFile); // delegate

                BlobClient blobClient =_blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
            }
            return datas;
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();

        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }
    }
}
