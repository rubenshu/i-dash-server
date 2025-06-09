using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Products.QueryHandlers;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using System.Threading;

namespace ItemDashServer.Application.Tests.Products.QueryHandlers;

public class GetProductsQueryHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ProductRepository _repository;

    public GetProductsQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("GetProductsHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new ProductRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ReturnsAllProducts()
    {
        _dbContext.Products.Add(new Product { Name = "P1", Description = "D1", Price = 1 });
        _dbContext.Products.Add(new Product { Name = "P2", Description = "D2", Price = 2 });
        await _dbContext.SaveChangesAsync();
        var handler = new GetProductsQueryHandler(_repository, _mapper);
        var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCountGreaterOrEqualTo(2);
    }
}
