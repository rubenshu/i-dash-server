using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Users.CommandHandlers;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Users.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application;
using System.Threading;

namespace ItemDashServer.Application.Users.CommandHandlers.Tests;

public class RefreshUserCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserRepository _repository;
    private readonly IAuthService _authService;

    public RefreshUserCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("RefreshUserHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new UserRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
        _authService = new DummyAuthService();
    }

    [Fact]
    public async Task Handle_RefreshesToken_WhenValid()
    {
        var user = new User { Username = "user", PasswordHash = new byte[1], PasswordSalt = new byte[1], RefreshToken = "token", RefreshTokenExpiry = System.DateTime.UtcNow.AddDays(1) };
        await _repository.AddAsync(user);
        var handler = new RefreshUserCommandHandler(_repository, _authService, _mapper);
        var cmd = new RefreshUserCommand("token");
        var (success, token, refreshToken, userDto) = await handler.Handle(cmd, CancellationToken.None);
        success.Should().BeTrue();
        userDto.Should().NotBeNull();
        refreshToken.Should().NotBeNull();
    }

    private class DummyAuthService : IAuthService
    {
        public string GenerateJwtToken(int userId, string username) => "dummy-token";
        public bool IsPasswordComplex(string password) => true;
    }
}
