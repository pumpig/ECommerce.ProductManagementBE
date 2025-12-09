using ECommerce.ProductManagement.Application.Services;
using ECommerce.ProductManagement.Application.Products;
using ECommerce.ProductManagement.Domain.Entities;
using ECommerce.ProductManagement.Domain.Common;
using ECommerce.ProductManagement.Application.Abstractions;
using FluentAssertions;
using MapsterMapper;
using Moq;
using Xunit;
using Microsoft.Extensions.Caching.Memory;

public sealed class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IMemoryCache> _memoryCacheMock = new();

    private ProductService CreateService()
        => new(_repoMock.Object, _fileServiceMock.Object, _mapperMock.Object, _memoryCacheMock.Object);

    [Fact]
    public async Task CreateAsync_Should_Return_Dto_When_Success()
    {
        // Arrange
        var service = CreateService();

        _repoMock
            .Setup(x => x.AddAsync(It.IsAny<Product>(), default))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(x => x.Map<ProductDto>(It.IsAny<Product>()))
            .Returns(new ProductDto(
                Guid.NewGuid(),
                "T-Shirt",
                "TS-01",
                Guid.NewGuid(),
                100,
                0,
                string.Empty));

        // Act
        var result = await service.CreateAsync(
            "T-Shirt",
            "TS-01",
            Guid.NewGuid(),
            100,
            100,
            string.Empty);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("T-Shirt");
    }

    [Fact]
    public async Task UpdatePriceAsync_Should_Return_NotFound_When_Product_Missing()
    {
        var service = CreateService();

        _repoMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Product?)null);

        var result = await service.UpdatePriceAsync(
            Guid.NewGuid(),
            200);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Product not found");
    }

    [Fact]
    public async Task GetPagedAsync_Should_Return_PagedResult()
    {
        var service = CreateService();

        var products = new List<Product>
        {
            Product.Create("A", "A1", Guid.NewGuid(), 10, 10, string.Empty).Value!,
            Product.Create("B", "B1", Guid.NewGuid(), 20, 20, string.Empty).Value!
        };

        _repoMock
            .Setup(x => x.GetPagedAsync(1, 10, string.Empty, default))
            .ReturnsAsync((products, 2));

        _mapperMock
            .Setup(x => x.Map<IReadOnlyList<ProductDto>>(products))
            .Returns(new List<ProductDto>
            {
                new ProductDto(
                    Guid.NewGuid(),
                    "A",
                    "SKU-A",
                    Guid.NewGuid(),
                    10,
                    5,
                    string.Empty),

                new ProductDto(
                    Guid.NewGuid(),
                    "B",
                    "SKU-B",
                    Guid.NewGuid(),
                    20,
                    3,
                    string.Empty)
            });

        var result = await service.GetPagedAsync(1, 10, string.Empty);

        result.Items.Count.Should().Be(2);
        result.HasNext.Should().BeFalse();
    }
}
