using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Products.QueryHandlers;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application;
using System.Threading;

namespace ItemDashServer.Application.Products.QueryHandlers.Tests;

public class GetProductByIdQueryHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ProductRepository _repository;

    public GetProductByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GetProductByIdHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new ProductRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsCorrectProduct()
    {
        var product = new Product { Name = "P1", Description = "D1", Price = 1 };
        await _repository.AddAsync(product);
        var handler = new GetProductByIdQueryHandler(_repository, _mapper);
        var result = await handler.Handle(new GetProductByIdQuery(product.Id), CancellationToken.None);
        result.Should().NotBeNull();
        result!.Name.Should().Be("P1");
    }
}
