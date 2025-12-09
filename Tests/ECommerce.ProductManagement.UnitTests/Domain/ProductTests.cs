using ECommerce.ProductManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

public sealed class ProductTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        // Act
        var result = Product.Create(
            "T-Shirt",
            "TSHIRT-001",
            Guid.NewGuid(),
            100,
            100,
            string.Empty);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Price.Should().Be(100);
        result.Value.StockQuantity.Should().Be(0);
    }

    [Fact]
    public void Create_Should_Fail_When_Price_Is_Negative()
    {
        var result = Product.Create(
            "T-Shirt",
            "TSHIRT-001",
            Guid.NewGuid(),
            -10,
            10,
            string.Empty);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Price must be >= 0");
    }

    [Fact]
    public void UpdatePrice_Should_Fail_When_Price_Is_Negative()
    {
        var product = Product.Create(
            "Shoes",
            "SH-001",
            Guid.NewGuid(),
            50, 
            50,
            string.Empty).Value!;

        var result = product.UpdatePrice(-1);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void UpdateStock_Should_Update_When_Quantity_Is_Valid()
    {
        var product = Product.Create(
            "Hat",
            "HAT-01",
            Guid.NewGuid(),
            30, 
            30,
            string.Empty).Value!;

        var result = product.UpdateStock(10);

        result.IsSuccess.Should().BeTrue();
        product.StockQuantity.Should().Be(10);
    }
}
