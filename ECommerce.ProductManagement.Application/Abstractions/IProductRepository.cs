using ECommerce.ProductManagement.Domain.Entities;

namespace ECommerce.ProductManagement.Application.Abstractions;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
       int pageIndex,
       int pageSize,
       string? search,
       CancellationToken cancellationToken);
}
