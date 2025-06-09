using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

namespace ItemDashServer.Application.Tests.Categories.CommandHandlers;

public class UpdateCategoryCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CategoryRepository _repository;

    public UpdateCategoryCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UpdateCategoryHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
    }

    [Fact]
    public async Task Handle_UpdatesCategory()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var userRepository = new UserRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, userRepository);
        var category = new Category { Name = "Old", Description = "D", Price = 1 };
        await categoryRepository.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        var handler = new UpdateCategoryCommandHandler(unitOfWork);
        var cmd = new UpdateCategoryCommand(category.Id, "New", "D", 2);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
        var updated = await categoryRepository.GetByIdAsync(category.Id);
        updated!.Name.Should().Be("New");
        updated.Price.Should().Be(2);
    }
}
