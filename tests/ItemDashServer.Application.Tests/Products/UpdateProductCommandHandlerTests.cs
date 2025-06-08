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

    [Fact]
    public async Task Handle_UpdatesProduct()
    {
        var product = new Product { Name = "Old", Description = "D", Price = 1 };
        await _repository.AddAsync(product);
        var handler = new UpdateProductCommandHandler(_repository);
        var cmd = new UpdateProductCommand(product.Id, "New", "D", 2);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
        var updated = await _repository.GetByIdAsync(product.Id);
        updated!.Name.Should().Be("New");
        updated.Price.Should().Be(2);
    }
}
