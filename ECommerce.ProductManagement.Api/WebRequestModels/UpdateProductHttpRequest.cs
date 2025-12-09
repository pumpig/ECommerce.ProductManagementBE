using ECommerce.ProductManagement.Application.Products;

namespace ECommerce.ProductManagement.Api.WebRequestModels
{
    public class UpdateProductHttpRequest : UpdateProductRequest
    {
        public IFormFile? Image { get; set; }
    }
}
