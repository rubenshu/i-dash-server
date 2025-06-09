using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Application.Categories;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<ILogger<CategoriesController>> _logger = new();
    private CategoriesController CreateController() => new(_mediator.Object, _logger.Object);

    [Fact]
    public async Task GetAll_ReturnsOkWithCategories()
    {
        _mediator.Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<CategoryDto>>.Success(new List<CategoryDto> { new() { Id = 1, Name = "C1" } }));
        var controller = CreateController();
        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var categories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(ok.Value);
        Assert.Single(categories);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryDto>.Success(new CategoryDto { Id = 1, Name = "C1" }));
        var controller = CreateController();
        var result = await controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var category = Assert.IsType<CategoryDto>(ok.Value);
        Assert.Equal(1, category.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryDto>.Failure("Not found"));
        var controller = CreateController();
        var result = await controller.GetById(1);
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
