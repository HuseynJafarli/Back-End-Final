﻿using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace YouPlay.Business.Utilities
{
    public static class FileManager
    {
        public static string SaveFile(this IFormFile formFile, string root, string folderName)
        {
            string fileName = formFile.FileName;

            fileName = fileName.Length > 64 ? fileName.Substring(fileName.Length - 64, 64) : fileName;

            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(root, folderName, fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            return fileName;
        }
        public static void DeleteFile(string root, string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            string fileNameToDelete = Path.GetFileName(fileName);

            string path = Path.Combine(root, folderName, fileNameToDelete);
            Console.WriteLine(path);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
