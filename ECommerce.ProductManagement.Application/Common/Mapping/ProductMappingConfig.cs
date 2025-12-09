using Mapster;
using ECommerce.ProductManagement.Application.Products;

namespace ECommerce.ProductManagement.Application.Common.Mapping;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Sku, src => src.Sku)
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.StockQuantity, src => src.StockQuantity);
    }
}
