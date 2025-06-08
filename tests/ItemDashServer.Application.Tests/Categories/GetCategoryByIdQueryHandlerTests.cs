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

public class GetCategoryByIdQueryHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CategoryRepository _repository;

    public GetCategoryByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GetCategoryByIdHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new CategoryRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsCorrectCategory()
    {
        var category = new Category { Name = "C1", Description = "D1", Price = 1 };
        await _repository.AddAsync(category);
        var handler = new GetCategoryByIdQueryHandler(_repository, _mapper);
        var result = await handler.Handle(new GetCategoryByIdQuery(category.Id), CancellationToken.None);
        result.Should().NotBeNull();
        result!.Name.Should().Be("C1");
    }
}
