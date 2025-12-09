using ECommerce.ProductManagement.Domain.Common;
using ECommerce.ProductManagement.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Sku { get; private set; } = default!;
    public Guid CategoryId { get; private set; }

    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    public string ImageUrl { get; private set; }

    public ICollection<ProductAttribute> Attributes { get; private set; }
        = new List<ProductAttribute>();

    public byte[] RowVersion { get; private set; } = default!;

    private Product() { }

    private Product(string name, string sku, Guid categoryId, decimal price, int stock, string imageUrl)
    {
        Id = Guid.NewGuid();
        Name = name;
        Sku = sku;
        CategoryId = categoryId;
        Price = price;
        StockQuantity = stock;
        ImageUrl = imageUrl;
    }

    public static Result<Product> Create(
        string name,
        string sku,
        Guid categoryId,
        decimal price,
        int stock,
        string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Product>.Failure("Product name is required");

        if (string.IsNullOrWhiteSpace(sku))
            return Result<Product>.Failure("SKU is required");

        if (categoryId == Guid.Empty)
            return Result<Product>.Failure("Category is required");

        if (price < 0)
            return Result<Product>.Failure("Price must be >= 0");

        if (stock < 0)
            return Result<Product>.Failure("Stock must be >= 0");

        return Result<Product>.Success(
            new Product(name, sku, categoryId, price, stock, imageUrl));
    }

    public static Result Update(Product product,
       string name,
       string sku,
       Guid categoryId,
       decimal price,
       int stock,
       string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Product>.Failure("Product name is required");

        if (string.IsNullOrWhiteSpace(sku))
            return Result<Product>.Failure("SKU is required");

        if (categoryId == Guid.Empty)
            return Result<Product>.Failure("Category is required");

        product.Name = name;
        product.Sku = sku;
        product.CategoryId = categoryId;
        product.Price = price;
        product.StockQuantity = stock;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            product.ImageUrl = imageUrl;
        }

        return Result.Success();
    }

    public Result UpdatePrice(decimal price)
    {
        if (price < 0)
            return Result.Failure("Price must be >= 0");

        Price = price;
        return Result.Success();
    }

    public Result UpdateStock(int quantity)
    {
        if (quantity < 0)
            return Result.Failure("Stock cannot be negative");

        StockQuantity = quantity;
        return Result.Success();
    }
}
