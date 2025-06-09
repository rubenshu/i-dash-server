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
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var userRepository = new UserRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, userRepository);
        var category = new Category { Name = "Del", Description = "D", Price = 1 };
        await categoryRepository.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        var handler = new DeleteCategoryCommandHandler(unitOfWork);
        var cmd = new DeleteCategoryCommand(category.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        var deleted = await categoryRepository.GetByIdAsync(category.Id);
        deleted.Should().BeNull();
    }
}
