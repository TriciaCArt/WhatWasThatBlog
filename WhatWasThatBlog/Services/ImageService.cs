using WhatWasThatBlog.Services.Interfaces;

namespace WhatWasThatBlog.Services

{
    public class ImageService : IImageService
    {
        #region Globals
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        private readonly string defaultImageSrc = "";
        #endregion
        #region Convert Byte Array to File
        public string ConvertByteArrayToFile(byte[] imageData, string extension)
        {
            if (imageData is null) return defaultImageSrc;
            try
            {
                string imageBase64Data = Convert.ToBase64String(imageData);
                return string.Format($"data:{extension};base64,{imageBase64Data}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Convert File to Byte Array
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            try
            {
                using MemoryStream memoryStream = new();
                await file.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                return byteFile;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
