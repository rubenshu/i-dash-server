using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;

namespace ItemDashServer.Application.Tests.Categories.Repositories;

public class CategoryRepositoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("CategoryRepoTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
    }

    [Fact]
    public async Task AddAndGetByIdAsync_Works()
    {
        var category = new Category { Name = "TestCat", Description = "Desc", Price = 1 };
        await _repository.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        var found = await _repository.GetByIdAsync(category.Id);
        found.Should().NotBeNull();
        found!.Name.Should().Be("TestCat");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAll()
    {
        _dbContext.Categories.Add(new Category { Name = "A", Description = "D", Price = 1 });
        _dbContext.Categories.Add(new Category { Name = "B", Description = "D", Price = 2 });
        await _dbContext.SaveChangesAsync();
        var all = await _repository.GetAllAsync();
        all.Should().HaveCountGreaterOrEqualTo(2);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesCategory()
    {
        var category = new Category { Name = "Old", Description = "D", Price = 1 };
        await _repository.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        category.Name = "New";
        await _repository.UpdateAsync(category);
        await _dbContext.SaveChangesAsync();
        var found = await _repository.GetByIdAsync(category.Id);
        found!.Name.Should().Be("New");
    }

    [Fact]
    public async Task DeleteAsync_RemovesCategory()
    {
        var category = new Category { Name = "Del", Description = "D", Price = 1 };
        await _repository.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        await _repository.DeleteAsync(category);
        await _dbContext.SaveChangesAsync();
        var found = await _repository.GetByIdAsync(category.Id);
        found.Should().BeNull();
    }
}
