using Microsoft.AspNetCore.Http;

namespace ECommerce.ProductManagement.Application.Abstractions
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string folder = "products", CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string filePath, CancellationToken cancellationToken = default);
        string GetFullPath(string relativePath);
    }
}
