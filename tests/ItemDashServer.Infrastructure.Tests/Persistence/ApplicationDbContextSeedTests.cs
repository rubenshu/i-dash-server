using System.Linq;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ItemDashServer.Infrastructure.Tests.Persistence;

public class ApplicationDbContextSeedTests
{
    private ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "SeedProductCategoriesTest")
            .Options;
        var context = new ApplicationDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void SeedProductCategories_CreatesExpectedProductCategoryLinks()
    {
        // Arrange
        var context = CreateInMemoryContext();
        ApplicationDbContextSeed.Seed(context);

        // Act
        var productCategories = context.ProductCategories.ToList();
        var fun = context.Categories.FirstOrDefault(c => c.Name == "Fun");
        var water = context.Categories.FirstOrDefault(c => c.Name == "Water");
        var outdoors = context.Categories.FirstOrDefault(c => c.Name == "Outdoors");
        var waterGun = context.Products.FirstOrDefault(p => p.Name == "Water Gun");
        var frisbee = context.Products.FirstOrDefault(p => p.Name == "Frisbee");
        var inflatablePool = context.Products.FirstOrDefault(p => p.Name == "Inflatable Pool");

        // Assert
        Assert.NotEmpty(productCategories);
        Assert.Contains(productCategories, pc => pc.ProductId == waterGun?.Id && pc.CategoryId == fun?.Id);
        Assert.Contains(productCategories, pc => pc.ProductId == frisbee?.Id && pc.CategoryId == outdoors?.Id);
        Assert.Contains(productCategories, pc => pc.ProductId == inflatablePool?.Id && pc.CategoryId == water?.Id);
        // ...add more asserts as needed for full coverage
    }
}
