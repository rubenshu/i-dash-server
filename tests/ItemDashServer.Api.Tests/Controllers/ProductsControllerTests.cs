using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Application.Products;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Products.QueryHandlers;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Api.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IGetProductsQueryHandler> _getProductsHandler = new();
    private readonly Mock<IGetProductByIdQueryHandler> _getProductByIdHandler = new();
    private readonly Mock<ILogger<ProductsController>> _logger = new();
    private ProductsController CreateController() => new();

    [Fact]
    public async Task GetAll_ReturnsOkWithProducts()
    {
        _getProductsHandler.Setup(h => h.ExecuteAsync(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<ProductDto>>.Success(new List<ProductDto> { new() { Id = 1, Name = "P1" } }));
        var controller = CreateController();
        var result = await controller.GetAll(_getProductsHandler.Object, default);
        var ok = Assert.IsType<OkObjectResult>(result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(ok.Value);
        Assert.Single(products);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        _getProductByIdHandler.Setup(h => h.ExecuteAsync(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Success(new ProductDto { Id = 1, Name = "P1" }));
        var controller = CreateController();
        var result = await controller.GetById(1, _getProductByIdHandler.Object, default);
        var ok = Assert.IsType<OkObjectResult>(result);
        var product = Assert.IsType<ProductDto>(ok.Value);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _getProductByIdHandler.Setup(h => h.ExecuteAsync(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure("Not found"));
        var controller = CreateController();
        var result = await controller.GetById(1, _getProductByIdHandler.Object, default);
        Assert.IsType<NotFoundResult>(result);
    }
}
