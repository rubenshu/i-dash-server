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

        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new() { Name = "Product 1", Description = "Description 1", Price = 10.99m },
                new() { Name = "Product 2", Description = "Description 2", Price = 20.99m },
                new() { Name = "Product 3", Description = "Description 3", Price = 30.99m }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
