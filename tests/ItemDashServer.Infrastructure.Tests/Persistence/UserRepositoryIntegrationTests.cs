using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Infrastructure.Tests.Persistence
{
    public class UserRepositoryIntegrationTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddAndGetByIdAsync_ShouldPersistUser()
        {
            var user = new User { Username = "testuser", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(user.Id);
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            var user = new User { Username = "todelete", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
            await _repository.AddAsync(user);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(user.Id);
            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
