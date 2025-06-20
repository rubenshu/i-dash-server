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
        await _dbContext.SaveChangesAsync();
        var handler = new GetProductByIdQueryHandler(_repository, _mapper);
        var result = await handler.ExecuteAsync(new GetProductByIdQuery(product.Id), CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("P1");
    }
}
