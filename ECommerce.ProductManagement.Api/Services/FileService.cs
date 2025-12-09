using ECommerce.ProductManagement.Application.Abstractions;

namespace ECommerce.ProductManagement.Api.Services
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadAsync(IFormFile file, string folder = "products", CancellationToken cancellationToken = default)
        {
            if (file == null) return string.Empty;

            string uploadsFolder = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            // Return relative path to store in database
            return Path.Combine(folder, fileName).Replace("\\", "/");
        }

        public Task<bool> DeleteAsync(string filePath, CancellationToken cancellationToken = default)
        {
            string fullPath = Path.Combine(_env.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public string GetFullPath(string relativePath)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return relativePath;

            return $"{request.Scheme}://{request.Host}//{relativePath}";
        }
    }
}
