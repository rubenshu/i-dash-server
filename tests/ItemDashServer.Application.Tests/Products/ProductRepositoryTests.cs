using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Application.Products.Repositories;

namespace ItemDashServer.Application.Products.Repositories.Tests;

public class ProductRepositoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ProductRepoTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new ProductRepository(_dbContext);
    }

    [Fact]
    public async Task AddAndGetByIdAsync_Works()
    {
        var product = new Product { Name = "Test", Description = "Desc", Price = 1.23M };
        await _repository.AddAsync(product);
        var found = await _repository.GetByIdAsync(product.Id);
        found.Should().NotBeNull();
        found!.Name.Should().Be("Test");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAll()
    {
        _dbContext.Products.Add(new Product { Name = "A", Description = "D", Price = 1 });
        _dbContext.Products.Add(new Product { Name = "B", Description = "D", Price = 2 });
        await _dbContext.SaveChangesAsync();
        var all = await _repository.GetAllAsync();
        all.Should().HaveCountGreaterOrEqualTo(2);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProduct()
    {
        var product = new Product { Name = "Old", Description = "D", Price = 1 };
        await _repository.AddAsync(product);
        product.Name = "New";
        await _repository.UpdateAsync(product);
        var found = await _repository.GetByIdAsync(product.Id);
        found!.Name.Should().Be("New");
    }

    [Fact]
    public async Task DeleteAsync_RemovesProduct()
    {
        var product = new Product { Name = "Del", Description = "D", Price = 1 };
        await _repository.AddAsync(product);
        await _repository.DeleteAsync(product);
        var found = await _repository.GetByIdAsync(product.Id);
        found.Should().BeNull();
    }
}
