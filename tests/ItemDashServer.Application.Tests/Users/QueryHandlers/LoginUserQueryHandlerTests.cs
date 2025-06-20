using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Users.QueryHandlers;
using ItemDashServer.Application.Users.Queries;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

namespace ItemDashServer.Application.Tests.Users.QueryHandlers;

public class LoginUserQueryHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserRepository _repository;

    public LoginUserQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("LoginUserHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new UserRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsUser_WhenCredentialsAreValid()
    {
        var password = "pass";
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var user = new User { Username = "user", PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)), PasswordSalt = hmac.Key };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var handler = new LoginUserQueryHandler(_repository, _mapper);
        var result = await handler.ExecuteAsync(new LoginUserQuery("user", password, null), CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Username.Should().Be("user");
    }
}
