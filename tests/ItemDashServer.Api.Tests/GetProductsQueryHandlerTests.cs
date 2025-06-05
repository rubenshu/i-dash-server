using FluentAssertions;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Products.QueryHandlers;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ItemDashServer.Application;

namespace ItemDashServer.Api.Tests
{
    public class GetProductsQueryHandlerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetProductsQueryHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Products.Add(new Domain.Entities.Product
            {
                Name = "TestProduct",
                Description = "Test Description",
                Price = 10.0m
            });
            _dbContext.SaveChanges();

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_ShouldReturnProducts()
        {
            var handler = new GetProductsQueryHandler(_dbContext, _mapper);
            var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().ContainSingle();
        }
    }
}