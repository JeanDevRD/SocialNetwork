﻿namespace SocialNetwork.Helpers
{
    public static class UploadFile
    {
        public static string? Uploader(IFormFile? file, int id, string folderName, bool Edit = false, string? imagePath = "")
        {

            if (file == null)
            {
                return string.Empty;
            }

            if (Edit && file == null)
            {
                return imagePath;
            }



            string basePath = $"Images/{folderName}/{id}";

            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();

            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string AllFilePath = Path.Combine(path, fileName);

            using (var stream = new FileStream(AllFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (Edit && !string.IsNullOrWhiteSpace(imagePath))
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldFileName = oldImagePart[^1];
                string completeOldPath = Path.Combine(path, oldFileName);

                if (File.Exists(completeOldPath))
                {
                    File.Delete(completeOldPath);
                }
            }

            return $"{basePath}/{fileName}";
        }

        public static bool Delete(int id, string folder)
        {
            string basePath = $"Images/{folder}/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{basePath}");

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            else
            {
                return false;
            }

            return true;

        }
    }
}
