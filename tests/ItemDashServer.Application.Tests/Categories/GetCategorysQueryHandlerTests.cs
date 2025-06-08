using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Categories.QueryHandlers;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application;
using System.Threading;

namespace ItemDashServer.Application.Categories.QueryHandlers.Tests;

public class GetCategoriesQueryHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CategoryRepository _repository;

    public GetCategoriesQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GetCategoriesHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsAllCategories()
    {
        _dbContext.Categories.Add(new Category { Name = "C1", Description = "D1", Price = 1 });
        _dbContext.Categories.Add(new Category { Name = "C2", Description = "D2", Price = 2 });
        await _dbContext.SaveChangesAsync();
        var handler = new GetCategoriesQueryHandler(_repository, _mapper);
        var result = await handler.Handle(new GetCategoriesQuery(), CancellationToken.None);
        result.Should().HaveCountGreaterOrEqualTo(2);
    }
}
