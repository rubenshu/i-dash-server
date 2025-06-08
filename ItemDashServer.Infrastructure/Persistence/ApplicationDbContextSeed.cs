using ItemDashServer.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ItemDashServer.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Users.Any())
        {
            var hmac = new System.Security.Cryptography.HMACSHA512();
            var user = new User
            {
                Username = "username",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("password")),
                PasswordSalt = hmac.Key
            };
            context.Users.Add(user);
            context.SaveChanges();
        }

        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Fun", Description = "Toys and games", Price = 0 },
                new() { Name = "Water", Description = "Water-related products", Price = 0 },
                new() { Name = "Outdoors", Description = "Outdoor equipment", Price = 0 }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new() { Name = "Water Gun", Description = "A toy gun that shoots water", Price = 15 },
                new() { Name = "Water Hose", Description = "A hose for watering plants", Price = 25 },
                new() { Name = "Frisbee", Description = "A flying disc for outdoor fun", Price = 10 },
                new() { Name = "Inflatable Pool", Description = "A small pool for kids", Price = 40 }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }

        if (!context.ProductCategories.Any())
        {
            var fun = context.Categories.FirstOrDefault(c => c.Name == "Fun");
            var water = context.Categories.FirstOrDefault(c => c.Name == "Water");
            var outdoors = context.Categories.FirstOrDefault(c => c.Name == "Outdoors");

            var waterGun = context.Products.FirstOrDefault(p => p.Name == "Water Gun");
            var waterHose = context.Products.FirstOrDefault(p => p.Name == "Water Hose");
            var frisbee = context.Products.FirstOrDefault(p => p.Name == "Frisbee");
            var inflatablePool = context.Products.FirstOrDefault(p => p.Name == "Inflatable Pool");

            var productCategories = new List<ProductCategory>
            {
                // Water Gun is in Fun and Water
                new() { ProductId = waterGun?.Id ?? 0, CategoryId = fun?.Id ?? 0 },
                new() { ProductId = waterGun?.Id ?? 0, CategoryId = water?.Id ?? 0 },

                // Water Hose is in Water and Outdoors
                new() { ProductId = waterHose?.Id ?? 0, CategoryId = water?.Id ?? 0 },
                new() { ProductId = waterHose?.Id ?? 0, CategoryId = outdoors?.Id ?? 0 },

                // Frisbee is in Fun and Outdoors
                new() { ProductId = frisbee?.Id ?? 0, CategoryId = fun?.Id ?? 0 },
                new() { ProductId = frisbee?.Id ?? 0, CategoryId = outdoors?.Id ?? 0 },

                // Inflatable Pool is in Fun, Water, and Outdoors
                new() { ProductId = inflatablePool?.Id ?? 0, CategoryId = fun?.Id ?? 0 },
                new() { ProductId = inflatablePool?.Id ?? 0, CategoryId = water?.Id ?? 0 },
                new() { ProductId = inflatablePool?.Id ?? 0, CategoryId = outdoors?.Id ?? 0 }
            };
            context.ProductCategories.AddRange(productCategories);
            context.SaveChanges();
        }
    }
}