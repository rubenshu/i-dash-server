using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Application.Products.CommandHandlers;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

namespace ItemDashServer.Application.Tests.Products.CommandHandlers;

public class UpdateProductCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProductRepository _repository;

    public UpdateProductCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UpdateProductHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new ProductRepository(_dbContext);
    }

/*     [Fact]
    public async Task Handle_UpdatesProduct()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var userRepository = new UserRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, userRepository);
        var product = new Product { Name = "Old", Description = "D", Price = 1 };
        await productRepository.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        var handler = new UpdateProductCommandHandler(unitOfWork);
        var cmd = new UpdateProductCommand(product.Id, "New", "D", 2);
        var result = await handler.HandleAsync(cmd, CancellationToken.None);
        var typedResult = (Result<bool>)result;
        typedResult.IsSuccess.Should().BeTrue();
        typedResult.Value.Should().BeTrue();
        var updated = await productRepository.GetByIdAsync(product.Id);
        updated!.Name.Should().Be("New");
        updated.Price.Should().Be(2);
    } */
}
