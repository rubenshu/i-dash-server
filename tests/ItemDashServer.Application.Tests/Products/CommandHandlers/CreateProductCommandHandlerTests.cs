using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Products.CommandHandlers;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Infrastructure.Persistence;
using System.Threading;

namespace ItemDashServer.Application.Tests.Products.CommandHandlers;

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

/*     [Fact]
    public async Task Handle_CreatesProduct()
    {
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var userRepository = new UserRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, userRepository);
        var handler = new CreateProductCommandHandler(unitOfWork, _mapper);
        var result = await handler.ExecuteAsync(new CreateProductCommand("P1", "D1", 1, null), CancellationToken.None);
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("P1");
    } */
}
