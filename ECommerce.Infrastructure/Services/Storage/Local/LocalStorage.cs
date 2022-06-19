﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Services.Storage.Local
{ 
    public class LocalStorage:ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

         async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                // todo log
                throw ex;
            }

        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            List<(string fileName, string path)> datas = new();

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            foreach (IFormFile file in files)
            {
                //string fileNewName = await FileRenameAsync(file.FileName);

                bool result = await CopyFileAsync($"{uploadPath}\\${file.Name}", file);
                datas.Add((file.Name, $"{path}\\${file.Name}"));
            }

            //if (results.TrueForAll(r => r.Equals(true))) return datas;
            return  datas;
            // todo Eğer ki yukarıdaki if geçerli değilse kullanıcıya hata mesajı dönecek bir exception yazılacak.
        }

        public async Task DeleteAsync(string path, string fileName) => File.Delete($"{path}\\{fileName}");

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f=>f.Name).ToList();
        }

        public bool HasFile(string path, string fileName) => File.Exists($"{path}\\{fileName}");
    }
}
