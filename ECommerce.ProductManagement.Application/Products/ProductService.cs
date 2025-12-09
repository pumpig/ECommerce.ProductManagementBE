using ECommerce.ProductManagement.Application.Abstractions;
using ECommerce.ProductManagement.Application.Common;
using ECommerce.ProductManagement.Application.Products;
using ECommerce.ProductManagement.Domain.Common;
using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.ProductManagement.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly HashSet<string> _productCacheKeys = new();

    public ProductService(
        IProductRepository productRepository,
        IFileService fileService,
        IMapper mapper,
        IMemoryCache cache)
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        string? search,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"products_{pageIndex}_{pageSize}_{search}";

        if (_cache.TryGetValue(cacheKey, out PagedResult<ProductDto> cachedProducts))
        {
            if(cachedProducts != null)
                return cachedProducts;
        }

        var (items, totalCount) =
            await _productRepository.GetPagedAsync(
                pageIndex,
                pageSize,
                search,
                cancellationToken);

        var dtoItems = items.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Sku,
            p.CategoryId,
            p.Price,
            p.StockQuantity,
            string.IsNullOrEmpty(p.ImageUrl) ? null : _fileService.GetFullPath(p.ImageUrl)
        )).ToList();


        var result = new PagedResult<ProductDto>(
            dtoItems,
            totalCount,
            pageIndex,
            pageSize);

        _cache.Set(cacheKey, dtoItems, TimeSpan.FromMinutes(5));

        lock (_productCacheKeys)
        {
            _productCacheKeys.Add(cacheKey);
        }

        return result;
    }

    public async Task<Result<ProductDto>> CreateAsync(
        string name,
        string sku,
        Guid categoryId,
        decimal price,
        int stock,
        string imageUrl,
        CancellationToken cancellationToken = default)
    {
        var result = Product.Create(
            name,
            sku,
            categoryId,
            price,
            stock,
            imageUrl);

        if (!result.IsSuccess)
            return Result<ProductDto>.Failure(result.Error!);

        await _productRepository.AddAsync(
            result.Value!,
            cancellationToken);

        var dto = _mapper.Map<ProductDto>(result.Value);

        ClearCache();

        return Result<ProductDto>.Success(dto);
    }

    public async Task<Result> UpdateAsync(
       Guid id,
       string name,
       string sku,
       Guid categoryId,
       decimal price,
       int stock,
       string imageUrl,
       CancellationToken cancellationToken = default)
    {
        var product = await _productRepository
             .GetByIdAsync(id, cancellationToken);

        if (product is null)
            return Result<ProductDto>.Failure("Product not found");

        var result = Product.Update(product,
            name,
            sku,
            categoryId,
            price,
            stock,
            imageUrl);

        if (!result.IsSuccess)
            return Result<ProductDto>.Failure(result.Error!);

        await _productRepository.UpdateAsync(
            product!,
            cancellationToken);

        ClearCache();

        return Result.Success();
    }

    public async Task<Result<ProductDto>> UpdatePriceAsync(
        Guid productId,
        decimal newPrice,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository
            .GetByIdAsync(productId, cancellationToken);

        if (product is null)
            return Result<ProductDto>.Failure("Product not found");

        var updateResult = product.UpdatePrice(newPrice);
        if (!updateResult.IsSuccess)
            return Result<ProductDto>.Failure(updateResult.Error!);

        await _productRepository.UpdateAsync(product, cancellationToken);

        ClearCache();

        return Result<ProductDto>.Success(
            _mapper.Map<ProductDto>(product));
    }

    public async Task<Result<ProductDto>> UpdateStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository
            .GetByIdAsync(productId, cancellationToken);

        if (product is null)
            return Result<ProductDto>.Failure("Product not found");

        var updateResult = product.UpdateStock(quantity);
        if (!updateResult.IsSuccess)
            return Result<ProductDto>.Failure(updateResult.Error!);

        await _productRepository.UpdateAsync(product, cancellationToken);

        ClearCache();

        return Result<ProductDto>.Success(
            _mapper.Map<ProductDto>(product));
    }

    public async Task<ProductDto?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository
            .GetByIdAsync(productId, cancellationToken);

        var result = product is null
            ? null
            : _mapper.Map<ProductDto>(product) with
            {
                ImageUrl = string.IsNullOrEmpty(product?.ImageUrl) ? null : _fileService.GetFullPath(product.ImageUrl)
            };

        return result;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await _productRepository
            .GetAllAsync(cancellationToken);

        return _mapper.Map<IReadOnlyList<ProductDto>>(products);
    }

    public async Task<Result> DeleteAsync(Guid productId,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository
             .GetByIdAsync(productId, cancellationToken);

        if (product is null)
            return Result.Failure("Product not found");

        await _productRepository.DeleteAsync(
            product,
            cancellationToken);

        ClearCache();

        return Result.Success();
    }

    private void ClearCache()
    {
        lock (_productCacheKeys)
        {
            foreach (var key in _productCacheKeys)
            {
                _cache.Remove(key);
            }
            _productCacheKeys.Clear(); // reset tracking
        }
    }
}
