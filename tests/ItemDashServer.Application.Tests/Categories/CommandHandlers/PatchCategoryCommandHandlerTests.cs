using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Text.Json;
using System.Threading;

namespace ItemDashServer.Application.Tests.Categories.CommandHandlers;

public class PatchCategoryCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CategoryRepository _repository;

    public PatchCategoryCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("PatchCategoryHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
    }

    [Fact]
    public async Task Handle_PatchesCategory()
    {
        var category = new Category { Name = "Patch", Description = "D", Price = 1 };
        await _repository.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        var patchDoc = JsonDocument.Parse("{\"Name\": \"Patched\", \"Price\": 99}");
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var userRepository = new UserRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, userRepository);
        var handler = new PatchCategoryCommandHandler(categoryRepository);
        var cmd = new PatchCategoryCommand(category.Id, patchDoc);
        var result = await handler.ExecuteAsync(cmd, CancellationToken.None);
        await _dbContext.SaveChangesAsync();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        var patched = await _repository.GetByIdAsync(category.Id);
        patched!.Name.Should().Be("Patched");
        patched.Price.Should().Be(99);
    }
}
