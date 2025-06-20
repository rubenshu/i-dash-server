using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Application.Categories;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.QueryHandlers;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IGetCategoriesQueryHandler> _getCategoriesHandler = new();
    private readonly Mock<IGetCategoryByIdQueryHandler> _getCategoryByIdHandler = new();
    private readonly Mock<ILogger<CategoriesController>> _logger = new();
    private CategoriesController CreateController() => new(_logger.Object);

    [Fact]
    public async Task GetAll_ReturnsOkWithCategories()
    {
        _getCategoriesHandler.Setup(h => h.ExecuteAsync(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<CategoryDto>>.Success(new List<CategoryDto> { new() { Id = 1, Name = "C1" } }));
        var controller = CreateController();
        var result = await controller.GetAll(_getCategoriesHandler.Object, default);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var categories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(ok.Value);
        Assert.Single(categories);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        _getCategoryByIdHandler.Setup(h => h.ExecuteAsync(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryDto>.Success(new CategoryDto { Id = 1, Name = "C1" }));
        var controller = CreateController();
        var result = await controller.GetById(1, _getCategoryByIdHandler.Object, default);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var category = Assert.IsType<CategoryDto>(ok.Value);
        Assert.Equal(1, category.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNull()
    {
        _getCategoryByIdHandler.Setup(h => h.ExecuteAsync(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryDto>.Failure("Not found"));
        var controller = CreateController();
        var result = await controller.GetById(1, _getCategoryByIdHandler.Object, default);
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
