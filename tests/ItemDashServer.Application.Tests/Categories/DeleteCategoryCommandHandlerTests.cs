using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

namespace ItemDashServer.Application.Categories.CommandHandlers.Tests;

public class DeleteCategoryCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CategoryRepository _repository;

    public DeleteCategoryCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("DeleteCategoryHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
    }

    [Fact]
    public async Task Handle_DeletesCategory()
    {
        var category = new Category { Name = "Del", Description = "D", Price = 1 };
        await _repository.AddAsync(category);
        var handler = new DeleteCategoryCommandHandler(_repository);
        var cmd = new DeleteCategoryCommand(category.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
        var deleted = await _repository.GetByIdAsync(category.Id);
        deleted.Should().BeNull();
    }
}
