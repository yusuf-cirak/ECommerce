using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Services.Storage
{
    public class StorageService:IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName,
            IFormFileCollection files) => await _storage.UploadAsync(pathOrContainerName, files);

        public async Task DeleteAsync(string pathOrContainerName, string fileName) =>
            await _storage.DeleteAsync(pathOrContainerName, fileName);

        public List<string> GetFiles(string pathOrContainerName) => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName) =>
            _storage.HasFile(pathOrContainerName, fileName);

        public string StorageName
        {
            get => _storage.GetType().Name;
        }
    }
}
