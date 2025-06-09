using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Infrastructure.Tests.Persistence
{
    public class CategoryRepositoryIntegrationTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new CategoryRepository(_context);
        }

        [Fact]
        public async Task AddAndGetByIdAsync_ShouldPersistCategory()
        {
            var category = new Category { Name = "Test Category", Description = "Desc", Price = 1 };
            await _repository.AddAsync(category);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(category.Id);
            Assert.NotNull(result);
            Assert.Equal("Test Category", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCategory()
        {
            var category = new Category { Name = "To Delete", Description = "Desc", Price = 2 };
            await _repository.AddAsync(category);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(category);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(category.Id);
            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
