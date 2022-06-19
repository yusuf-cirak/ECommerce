using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ECommerce.Infrastructure.Services
{
    public class FileService
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<(string fileName, string filePath)>> UploadAsync(string path, IFormFileCollection files)
        {
            List<(string fileName, string path)> datas = new();
            List<bool> results = new();


            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(file.FileName);

                bool result = await CopyFileAsync($"{uploadPath}\\${fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\${fileNewName}"));
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true))) return datas;
            return null;
            // todo Eğer ki yukarıdaki if geçerli değilse kullanıcıya hata mesajı dönecek bir exception yazılacak.
        }

        async Task<string> FileRenameAsync(string fileName)
        {
            // first olsa da olmasa da return $"{newFileName}-{date}{extension}";
            // date first=true veya false'a bağımlı değil
            string extension = Path.GetExtension(fileName);
            string fileNameWithoutExtension=Path.GetFileNameWithoutExtension(fileName);

            string newFileName = NameOperation.CharacterRegulatory(fileNameWithoutExtension);
            string date = DateTime.Now.ToString("ddMMyyyyHHmmsss");

            if (File.Exists($"{newFileName}-{date}{extension}")) return await FileRenameAsync(newFileName);

            return $"{newFileName}-{date}{extension}";
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
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


    }
}
