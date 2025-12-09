using FluentValidation;
using ECommerce.ProductManagement.Application.Products;

namespace ECommerce.ProductManagement.Application.Products.Validators;

public sealed class UpdateProductPriceRequestValidator
    : AbstractValidator<UpdateProductPriceRequest>
{
    public UpdateProductPriceRequestValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0");
    }
}
