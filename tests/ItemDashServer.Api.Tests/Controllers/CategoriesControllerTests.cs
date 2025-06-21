using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Application.Categories;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.QueryHandlers;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IGetCategoriesQueryHandler> _getCategoriesHandler = new();
    private readonly Mock<IGetCategoryByIdQueryHandler> _getCategoryByIdHandler = new();
    private readonly Mock<ILogger<CategoriesController>> _logger = new();
    private readonly Mock<ICreateCategoryCommandHandler> _createCategoryHandler = new();
    private readonly Mock<IUpdateCategoryCommandHandler> _updateCategoryHandler = new();
    private CategoriesController CreateController() => new();

    [Fact]
    public async Task GetAll_ReturnsOkWithCategories()
    {
        _getCategoriesHandler.Setup(h => h.ExecuteAsync(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<CategoryDto>>.Success(new List<CategoryDto> { new() { Id = 1, Name = "C1" } }));
        var controller = CreateController();
        var result = await controller.GetAll(_getCategoriesHandler.Object, default);
        var ok = Assert.IsType<OkObjectResult>(result);
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
        var ok = Assert.IsType<OkObjectResult>(result);
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
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenSuccessful()
    {
        var category = new CategoryDto { Id = 1, Name = "C1" };
        _createCategoryHandler.Setup(h => h.HandleAsync(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryDto>.Success(category));
        var controller = CreateController();
        var product = new Product { Name = "C1", Description = "desc", Price = 1m };
        var result = await controller.Create(product, _createCategoryHandler.Object, default);
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var value = Assert.IsType<CategoryDto>(created.Value);
        Assert.Equal(category.Id, value.Id);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelInvalid()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Name", "Required");
        var product = new Product { Name = "C1", Description = "desc", Price = 1m };
        var result = await controller.Create(product, _createCategoryHandler.Object, default);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenHandlerFails()
    {
        _createCategoryHandler.Setup(h => h.HandleAsync(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<CategoryDto>.Failure("fail"));
        var controller = CreateController();
                var product = new Product { Name = "C1", Description = "desc", Price = 1m };
        var result = await controller.Create(product, _createCategoryHandler.Object, default);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("fail", badRequest.Value);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenSuccessful()
    {
        _updateCategoryHandler.Setup(h => h.HandleAsync(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Success(true));
        var controller = CreateController();
        var command = new UpdateCategoryCommand(1, "C1", "desc", 1m);
        var result = await controller.Update(1, command, _updateCategoryHandler.Object, default);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenModelInvalid()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Name", "Required");
        var command = new UpdateCategoryCommand(1, "", "", 0m);
        var result = await controller.Update(1, command, _updateCategoryHandler.Object, default);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var controller = CreateController();
        var command = new UpdateCategoryCommand(2, "C1", "desc", 1m);
        var result = await controller.Update(1, command, _updateCategoryHandler.Object, default);
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ID in URL does not match ID in body.", badRequest.Value);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenHandlerFails()
    {
        _updateCategoryHandler.Setup(h => h.HandleAsync(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Success(false));
        var controller = CreateController();
        var command = new UpdateCategoryCommand(1, "C1", "desc", 1m);
        var result = await controller.Update(1, command, _updateCategoryHandler.Object, default);
        Assert.IsType<NotFoundResult>(result);
    }
}
