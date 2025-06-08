using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Text.Json;
using System.Threading;

namespace ItemDashServer.Application.Categories.CommandHandlers.Tests;

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
        var patchDoc = JsonDocument.Parse("{\"Name\": \"Patched\", \"Price\": 99}");
        var handler = new PatchCategoryCommandHandler(_repository);
        var cmd = new PatchCategoryCommand(category.Id, patchDoc);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
        var patched = await _repository.GetByIdAsync(category.Id);
        patched!.Name.Should().Be("Patched");
        patched.Price.Should().Be(99);
    }
}
