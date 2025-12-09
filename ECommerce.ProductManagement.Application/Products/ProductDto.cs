namespace ECommerce.ProductManagement.Application.Products;

public record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    Guid CategoryId,
    decimal Price,
    int StockQuantity,
    string ImageUrl
);
