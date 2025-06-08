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
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;

namespace ItemDashServer.Application.Products.CommandHandlers.Tests;

public class PatchProductCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProductRepository _repository;

    public PatchProductCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("PatchProductHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new ProductRepository(_dbContext);
    }

    [Fact]
    public async Task Handle_PatchesProduct()
    {
        var product = new Product { Name = "Patch", Description = "D", Price = 1 };
        await _repository.AddAsync(product);
        var patchDoc = JsonDocument.Parse("{\"Name\": \"Patched\", \"Price\": 99}");
        var handler = new PatchProductCommandHandler(_repository);
        var cmd = new PatchProductCommand(product.Id, patchDoc);
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
        var patched = await _repository.GetByIdAsync(product.Id);
        patched!.Name.Should().Be("Patched");
        patched.Price.Should().Be(99);
    }
}
