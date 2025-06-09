using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Products.CommandHandlers;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

namespace ItemDashServer.Application.Tests.Products.CommandHandlers;

public class DeleteProductCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProductRepository _repository;

    public DeleteProductCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("DeleteProductHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new ProductRepository(_dbContext);
    }

    [Fact]
    public async Task Handle_DeletesProduct()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var userRepository = new UserRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, userRepository);
        var product = new Product { Name = "Del", Description = "D", Price = 1 };
        await productRepository.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        var handler = new DeleteProductCommandHandler(unitOfWork);
        var cmd = new DeleteProductCommand(product.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
        var deleted = await productRepository.GetByIdAsync(product.Id);
        deleted.Should().BeNull();
    }
}
