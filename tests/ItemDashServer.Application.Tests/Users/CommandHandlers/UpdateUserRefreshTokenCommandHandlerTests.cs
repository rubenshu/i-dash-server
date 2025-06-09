using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Application.Users.CommandHandlers;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

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
    public async Task Handle_UpdatesRefreshToken()
    {
        var user = new User { Username = "user", PasswordHash = new byte[1], PasswordSalt = new byte[1] };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync(); // Ensure user is persisted
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
        var handler = new UpdateUserRefreshTokenCommandHandler(unitOfWork);
        var cmd = new UpdateUserRefreshTokenCommand(user.Id, "refresh", System.DateTime.UtcNow.AddDays(1));
        await handler.Handle(cmd, CancellationToken.None);
        var updated = await _repository.GetByIdAsync(user.Id);
        updated!.RefreshToken.Should().Be("refresh");
    }
}
