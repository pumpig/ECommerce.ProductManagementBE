using ECommerce.ProductManagement.Application.Products;

namespace ECommerce.ProductManagement.Api.WebRequestModels
{
    public class CreateProductHttpRequest : CreateProductRequest
    {
        public IFormFile? Image { get; set; }
    }
}
