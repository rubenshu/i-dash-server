using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Products.CommandHandlers;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application;
using System.Threading;

namespace ItemDashServer.Application.Products.CommandHandlers.Tests;

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
        var product = new Product { Name = "Del", Description = "D", Price = 1 };
        await _repository.AddAsync(product);
        var handler = new DeleteProductCommandHandler(_repository);
        var cmd = new DeleteProductCommand(product.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
        var deleted = await _repository.GetByIdAsync(product.Id);
        deleted.Should().BeNull();
    }
}
