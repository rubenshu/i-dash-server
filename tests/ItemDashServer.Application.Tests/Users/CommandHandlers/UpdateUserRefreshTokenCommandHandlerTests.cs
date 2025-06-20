using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ItemDashServer.Application.Users.CommandHandlers;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Application.Tests.Users.CommandHandlers;

public class UpdateUserRefreshTokenCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserRepository _repository;

    public UpdateUserRefreshTokenCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UpdateUserRefreshTokenHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new UserRepository(_dbContext);
    }

    [Fact]
    public async Task ExecuteAsync_UpdatesRefreshToken()
    {
        var user = new User { Username = "user", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync(); // Ensure user is persisted
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var cmd = new UpdateUserRefreshTokenCommand(user.Id, "refresh", System.DateTime.UtcNow.AddDays(1));
        await handler.ExecuteAsync(cmd, CancellationToken.None);
        var updated = await _repository.GetByIdAsync(user.Id);
        updated!.RefreshToken.Should().Be("refresh");
    }

    [Fact]
    public async Task ExecuteAsync_UserNotFound_DoesNothing()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var nonExistentUserId = 9999;
        var cmd = new UpdateUserRefreshTokenCommand(nonExistentUserId, "refresh", System.DateTime.UtcNow.AddDays(1));
        // Should not throw
        await handler.ExecuteAsync(cmd, CancellationToken.None);
        var updated = await _repository.GetByIdAsync(nonExistentUserId);
        updated.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_UserExists_UpdatesRefreshTokenAndExpiry_ReturnsSuccess()
    {
        var user = new User { Username = "user", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var expiry = DateTime.UtcNow.AddDays(1);
        var cmd = new UpdateUserRefreshTokenCommand(user.Id, "refresh", expiry);
        var result = await handler.ExecuteAsync(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        var updated = await _repository.GetByIdAsync(user.Id);
        updated!.RefreshToken.Should().Be("refresh");
        updated.RefreshTokenExpiry.Should().BeCloseTo(expiry, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task ExecuteAsync_UserNotFound_ReturnsFailure()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var cmd = new UpdateUserRefreshTokenCommand(9999, "refresh", DateTime.UtcNow.AddDays(1));
        var result = await handler.ExecuteAsync(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User not found");
    }

    [Fact]
    public async Task ExecuteAsync_NullRefreshToken_UpdatesToNull()
    {
        var user = new User { Username = "user2", PasswordHash = new byte[1], PasswordSalt = new byte[1], RefreshToken = "old", RefreshTokenExpiry = DateTime.UtcNow };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var cmd = new UpdateUserRefreshTokenCommand(user.Id, null, DateTime.UtcNow.AddDays(2));
        var result = await handler.ExecuteAsync(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        var updated = await _repository.GetByIdAsync(user.Id);
        updated!.RefreshToken.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_NullExpiry_UpdatesToNull()
    {
        var user = new User { Username = "user3", PasswordHash = new byte[1], PasswordSalt = new byte[1], RefreshToken = "tok" };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var cmd = new UpdateUserRefreshTokenCommand(user.Id, "tok", null);
        var result = await handler.ExecuteAsync(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        var updated = await _repository.GetByIdAsync(user.Id);
        updated!.RefreshTokenExpiry.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_EmptyRefreshToken_UpdatesToEmpty()
    {
        var user = new User { Username = "user4", PasswordHash = new byte[1], PasswordSalt = new byte[1], RefreshToken = "tok" };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var cmd = new UpdateUserRefreshTokenCommand(user.Id, string.Empty, DateTime.UtcNow.AddDays(1));
        var result = await handler.ExecuteAsync(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        var updated = await _repository.GetByIdAsync(user.Id);
        updated!.RefreshToken.Should().Be("");
    }

    [Fact]
    public async Task ExecuteAsync_CommitFails_ReturnsFailure()
    {
        var user = new User { Username = "user5", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(u => u.Users.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        uowMock.Setup(u => u.Users.UpdateAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uowMock.Setup(u => u.CommitAsync()).ThrowsAsync(new Exception("DB error"));
        var handler = new UpdateUserRefreshTokenCommandHandler(uowMock.Object);
        var cmd = new UpdateUserRefreshTokenCommand(user.Id, "fail", DateTime.UtcNow.AddDays(1));
        var result = await handler.ExecuteAsync(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("DB error");
    }
}
