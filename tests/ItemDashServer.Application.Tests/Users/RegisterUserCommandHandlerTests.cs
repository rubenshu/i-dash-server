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

public class RegisterUserCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserRepository _repository;

    public RegisterUserCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("RegisterUserHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new UserRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_RegistersUser()
    {
        var handler = new RegisterUserCommandHandler(_repository, _mapper);
        var result = await handler.Handle(new RegisterUserCommand("newuser", "pass"), CancellationToken.None);
        result.Should().NotBeNull();
        result.Username.Should().Be("newuser");
    }
}
