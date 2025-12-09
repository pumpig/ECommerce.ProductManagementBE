namespace ECommerce.ProductManagement.Application.Products
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
