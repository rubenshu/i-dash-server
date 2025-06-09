using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Infrastructure.Persistence;
using System.Threading;
using ItemDashServer.Application.Categories.CommandHandlers;

namespace ItemDashServer.Application.Tests.Categories.CommandHandlers;

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
        var categoryRepository = new CategoryRepository(_dbContext);
        var productRepository = new ProductRepository(_dbContext);
        var userRepository = new UserRepository(_dbContext);
        var unitOfWork = new UnitOfWork(_dbContext, categoryRepository, productRepository, userRepository);
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        var mapper = config.CreateMapper();
        var handler = new CreateCategoryCommandHandler(unitOfWork, mapper);
        var result = await handler.Handle(new CreateCategoryCommand("C1", "D1", 1), CancellationToken.None);
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("C1");
    }
}
