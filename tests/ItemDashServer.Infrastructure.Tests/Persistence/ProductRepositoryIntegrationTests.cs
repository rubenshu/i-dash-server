using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Infrastructure.Tests.Persistence
{
    public class ProductRepositoryIntegrationTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task AddAndGetByIdAsync_ShouldPersistProduct()
        {
            var product = new Product { Name = "Test Product", Description = "A test product", Price = 10 };
            await _repository.AddAsync(product);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(product.Id);
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            var product = new Product { Name = "To Delete", Description = "To be deleted", Price = 5 };
            await _repository.AddAsync(product);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(product);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(product.Id);
            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
