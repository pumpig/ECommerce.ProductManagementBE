using ECommerce.ProductManagement.Application.Common;
using ECommerce.ProductManagement.Application.Products;
using ECommerce.ProductManagement.Domain.Common;

namespace ECommerce.ProductManagement.Application.Abstractions;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> CreateAsync(
        string name,
        string sku,
        Guid categoryId,
        decimal price,
        int stock,
        string imageUrl,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(
        Guid productId,
        string name,
        string sku,
        Guid categoryId,
        decimal price,
        int stock,
        string imageUrl,
        CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> UpdatePriceAsync(
        Guid productId,
        decimal newPrice,
        CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> UpdateStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);

    Task<ProductDto?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid productId,
        CancellationToken cancellationToken = default);
}