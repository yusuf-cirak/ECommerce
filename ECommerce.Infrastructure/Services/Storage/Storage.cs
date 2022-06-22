using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Infrastructure.Operations;

namespace ECommerce.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        protected async Task<string> FileRenameAsync(string pathOrContainerName,string fileName,HasFile hasFileMethod)
        {
            string extension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            //string newFileName = NameOperation.CharacterRegulatory(fileNameWithoutExtension);
            string date = DateTime.Now.ToString("ddMMyyyyHHmmsss");

            //if (File.Exists($"{newFileName}-{date}{extension}")) return await FileRenameAsync(newFileName);
            if (hasFileMethod(pathOrContainerName,fileName)) return await FileRenameAsync(pathOrContainerName, fileNameWithoutExtension, hasFileMethod);

            return $"{fileNameWithoutExtension}-{date}{extension}";
        }

    }
}
