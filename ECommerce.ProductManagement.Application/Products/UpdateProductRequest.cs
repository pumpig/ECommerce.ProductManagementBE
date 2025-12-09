namespace ECommerce.ProductManagement.Application.Products
{
    public class UpdateProductRequest
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
