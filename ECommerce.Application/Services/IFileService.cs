using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Services
{
    public interface IFileService
    {

        Task<List<(string fileName,string filePath)>> UploadAsync(string path,IFormFileCollection files);
        // wwwroot/path
        // Gelen dosyaları Request.Form.Files ile yakalıyorduk.
        // Tuple listesi dönen bir List ile fileName ve filePath'leri döndüreceğiz.

        Task<bool> CopyFileAsync(string path,IFormFile file);

    }
}
