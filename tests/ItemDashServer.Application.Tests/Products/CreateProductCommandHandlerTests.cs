using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Products.CommandHandlers;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Application;
using System.Threading;

namespace ItemDashServer.Application.Products.CommandHandlers.Tests;

public class CreateProductCommandHandlerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ProductRepository _repository;

    public CreateProductCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("CreateProductHandlerTestDb")
            .Options;
        _dbContext = new ApplicationDbContext(options);
        _repository = new ProductRepository(_dbContext);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_CreatesProduct()
    {
        var handler = new CreateProductCommandHandler(_repository, _mapper);
        var result = await handler.Handle(new CreateProductCommand("P1", "D1", 1), CancellationToken.None);
        result.Should().NotBeNull();
        result.Name.Should().Be("P1");
    }
}
