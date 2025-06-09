using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Application.Products;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Api.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<ILogger<ProductsController>> _logger = new();
    private ProductsController CreateController() => new(_mediator.Object, _logger.Object);

    [Fact]
    public async Task GetAll_ReturnsOkWithProducts()
    {
        _mediator.Setup(m => m.Send(It.IsAny<GetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<ProductDto>>.Success(new List<ProductDto> { new() { Id = 1, Name = "P1" } }));
        var controller = CreateController();
        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(ok.Value);
        Assert.Single(products);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        _mediator.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Success(new ProductDto { Id = 1, Name = "P1" }));
        var controller = CreateController();
        var result = await controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var product = Assert.IsType<ProductDto>(ok.Value);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _mediator.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<ProductDto>.Failure("Not found"));
        var controller = CreateController();
        var result = await controller.GetById(1);
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
