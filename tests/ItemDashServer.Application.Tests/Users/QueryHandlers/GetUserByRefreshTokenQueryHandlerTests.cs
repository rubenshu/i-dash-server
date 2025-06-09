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

public class GetUserByRefreshTokenQueryHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserRepository _repository;

    public GetUserByRefreshTokenQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GetUserByRefreshTokenHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new UserRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsUser_WhenRefreshTokenMatches()
    {
        var user = new User { Username = "user", PasswordHash = new byte[1], PasswordSalt = new byte[1], RefreshToken = "token" };
        await _repository.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var handler = new GetUserByRefreshTokenQueryHandler(_repository, _mapper);
        var result = await handler.Handle(new GetUserByRefreshTokenQuery("token"), CancellationToken.None);
        result.Should().NotBeNull();
        result!.Username.Should().Be("user");
    }
}
