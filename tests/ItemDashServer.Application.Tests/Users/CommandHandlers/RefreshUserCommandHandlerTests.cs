using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Users.CommandHandlers;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Services;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

namespace ItemDashServer.Application.Tests.Users.CommandHandlers;

public class RefreshUserCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserRepository _repository;
    private readonly IAuthService _authService;
    private readonly UnitOfWork _unitOfWork;

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
        // Add missing repositories for UnitOfWork
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        _unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, _repository);
    }

    [Fact]
    public async Task Handle_RefreshesToken_WhenValid()
    {
        var user = new User { Username = "user", PasswordHash = new byte[1], PasswordSalt = new byte[1], RefreshToken = "token", RefreshTokenExpiry = System.DateTime.UtcNow.AddDays(1) };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync(); // Ensure user is persisted
        var handler = new RefreshUserCommandHandler(_unitOfWork, _authService, _mapper);
        var cmd = new RefreshUserCommand("token");
        var result = await handler.Handle(cmd, CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Token.Should().Be("dummy-token");
        result.Value.RefreshToken.Should().NotBeNull();
        result.Value.User.Should().NotBeNull();
    }

    private class DummyAuthService : IAuthService
    {
        public string GenerateJwtToken(int userId, string username) => "dummy-token";
        public bool IsPasswordComplex(string password) => true;
    }
}
