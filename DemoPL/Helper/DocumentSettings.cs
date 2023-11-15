using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DemoPL.Helper
{
    public class DocumentSettings
    {
        //Its better not to upload files to databse instead we save the file path in the data base 
        // for files that is saved on Server hard disk
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            //1.Get Located Folder 
            // string forlderPath = "D:\\Dotnet Course Route\\Sessions\\07-ASP.NET Core MVC\\Session 02\\Demos\\G02 Demo Solution\\DemoPL\\wwwroot\\files\\Images\\"
            //string forlderPath = Directory.GetCurrentDirectory() + "wwwroot\\files" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            //2.Get FileName Make it Unique
            string fileName =  $"{Guid.NewGuid()}{file?.FileName}";

            //3.Get file path 
            string filepath = Path.Combine(folderPath, fileName);
            //4.Save file as stream :[Data per Time]
            using var fs =  new FileStream(filepath, FileMode.Create);

           await file?.CopyToAsync(fs);

            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
