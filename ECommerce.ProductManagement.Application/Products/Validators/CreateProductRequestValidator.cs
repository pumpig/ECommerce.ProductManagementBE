using FluentValidation;

namespace ECommerce.ProductManagement.Application.Products;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Sku)
            .NotEmpty()
            .Matches("^[A-Za-z0-9_-]+$")
            .WithMessage("SKU only allows alphanumeric, hyphen, underscore.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
