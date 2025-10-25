using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PersonalWebsiteBFF.Core.Interfaces;
using Supabase;

namespace PersonalWebsiteBFF.Core.Services
{
    public class StorageService : IStorageService
    {
        private readonly Client _supabase;
        private readonly string _bucketName;

        public StorageService(IConfiguration config)
        {
            string url = config["Supabase:Url"]!;
            string key = config["Supabase:ServiceKey"]!;

            _bucketName = config["Supabase:BucketName"]!;

            _supabase = new Client(url, key);
            _supabase.InitializeAsync().Wait();
        }

        public async Task<string> UploadPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            var filePath = $"{Guid.NewGuid()}_{file.FileName}";

            await _supabase.Storage.From(_bucketName).Upload(fileBytes, filePath);

            var publicUrl = _supabase.Storage.From(_bucketName).GetPublicUrl(filePath);
            return publicUrl;
        }
    }
}
