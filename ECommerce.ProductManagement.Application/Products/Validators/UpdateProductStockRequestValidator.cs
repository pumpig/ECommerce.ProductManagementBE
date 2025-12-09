using FluentValidation;
using ECommerce.ProductManagement.Application.Products;

namespace ECommerce.ProductManagement.Application.Products.Validators;

public sealed class UpdateProductStockRequestValidator
    : AbstractValidator<UpdateProductStockRequest>
{
    public UpdateProductStockRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative");
    }
}
