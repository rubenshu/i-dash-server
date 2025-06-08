using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application;
using System.Threading;

namespace ItemDashServer.Application.Categories.CommandHandlers.Tests;

public class CreateCategoryCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CategoryRepository _repository;

    public CreateCategoryCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("CreateCategoryHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_CreatesCategory()
    {
        var handler = new CreateCategoryCommandHandler(_repository, _mapper);
        var result = await handler.Handle(new CreateCategoryCommand("C1", "D1", 1), CancellationToken.None);
        result.Should().NotBeNull();
        result.Name.Should().Be("C1");
    }
}
