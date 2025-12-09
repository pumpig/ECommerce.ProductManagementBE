using ECommerce.ProductManagement.Api.WebRequestModels;
using ECommerce.ProductManagement.Application.Abstractions;
using ECommerce.ProductManagement.Application.Products;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IFileService _fileService;

    public ProductsController(IProductService productService, IFileService fileService)
    {
        _productService = productService;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _productService.GetPagedAsync(
            pageIndex,
            pageSize,
            search,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetByIdAsync(
            id,
            cancellationToken);

        return product is null
            ? NotFound()
            : Ok(product);
    }


    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProduct(
        [FromForm] CreateProductHttpRequest request,
        CancellationToken cancellationToken = default)
    {
        string? imageUrl = null;

        if (request.Image != null)
        {
            imageUrl = await _fileService.UploadAsync(request.Image);
        }

        var result = await _productService.CreateAsync(
          request.Name,
          request.Sku,
          request.CategoryId,
          request.Price,
          request.StockQuantity,
          imageUrl ?? string.Empty,
          cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            result.Value);
    }

    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct(
      [FromForm] UpdateProductHttpRequest request,
      CancellationToken cancellationToken = default)
    {
        string? imageUrl = null;

        if (request.Image != null)
        {
            imageUrl = await _fileService.UploadAsync(request.Image);
        }

        var result = await _productService.UpdateAsync(
            request.ProductId,
            request.Name,
            request.Sku,
            request.CategoryId,
            request.Price,
            request.StockQuantity,
            imageUrl ?? string.Empty,
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/price")]
    public async Task<IActionResult> UpdatePrice(
        Guid id,
        [FromBody] UpdateProductPriceRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _productService.UpdatePriceAsync(
            id,
            request.Price,
            cancellationToken);

        if (!result.IsSuccess)
            return result.Error == "Product not found"
                ? NotFound()
                : BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/stock")]
    public async Task<IActionResult> UpdateStock(
        Guid id,
        [FromBody] UpdateProductStockRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _productService.UpdateStockAsync(
            id,
            request.Quantity,
            cancellationToken);

        if (!result.IsSuccess)
            return result.Error == "Product not found"
                ? NotFound()
                : BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productService.DeleteAsync(id, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return NoContent();
    }
}
