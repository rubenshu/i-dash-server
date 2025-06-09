using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;

namespace ItemDashServer.Application.Tests.Users.Repositories;

public class UserRepositoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UserRepoTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new UserRepository(_dbContext);
    }

    [Fact]
    public async Task AddAndGetByIdAsync_Works()
    {
        var user = new User { Username = "user1", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var found = await _repository.GetByIdAsync(user.Id);
        found.Should().NotBeNull();
        found!.Username.Should().Be("user1");
    }

    [Fact]
    public async Task GetByUsernameAsync_Works()
    {
        var user = new User { Username = "user2", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var found = await _repository.GetByUsernameAsync("user2");
        found.Should().NotBeNull();
        found!.Username.Should().Be("user2");
    }

    [Fact]
    public async Task GetByRefreshTokenAsync_Works()
    {
        var user = new User { Username = "user3", PasswordHash = new byte[1], PasswordSalt = new byte[1], RefreshToken = "token123" };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var found = await _repository.GetByRefreshTokenAsync("token123");
        found.Should().NotBeNull();
        found!.RefreshToken.Should().Be("token123");
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUser()
    {
        var user = new User { Username = "user4", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        user.Username = "user4-updated";
        await _repository.UpdateAsync(user);
        await _dbContext.SaveChangesAsync();
        var found = await _repository.GetByIdAsync(user.Id);
        found!.Username.Should().Be("user4-updated");
    }
}
