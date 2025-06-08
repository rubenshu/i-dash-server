using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Categorys.QueryHandlers;
using ItemDashServer.Application.Categorys.Queries;
using ItemDashServer.Application.Categorys.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application;
using System.Threading;

namespace ItemDashServer.Application.Categorys.QueryHandlers.Tests;

public class GetCategorysQueryHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CategoryRepository _repository;

    public GetCategorysQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GetCategorysHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsAllCategories()
    {
        _dbContext.Categorys.Add(new Category { Name = "C1", Description = "D1", Price = 1 });
        _dbContext.Categorys.Add(new Category { Name = "C2", Description = "D2", Price = 2 });
        await _dbContext.SaveChangesAsync();
        var handler = new GetCategorysQueryHandler(_repository, _mapper);
        var result = await handler.Handle(new GetCategorysQuery(), CancellationToken.None);
        result.Should().HaveCountGreaterOrEqualTo(2);
    }
}
