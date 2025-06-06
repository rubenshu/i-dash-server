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

        if (!context.Categorys.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Electronics", Description = "Electronic items", Price = 0 },
                new() { Name = "Books", Description = "Books and literature", Price = 0 }
            };
            context.Categorys.AddRange(categories);
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            var electronics = context.Categorys.FirstOrDefault(c => c.Name == "Electronics");
            var books = context.Categorys.FirstOrDefault(c => c.Name == "Books");

            var products = new List<Product>
            {
                new() { Name = "Laptop", Description = "A portable computer", Price = 1200 },
                new() { Name = "Novel", Description = "A fiction book", Price = 20 }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }

        if (!context.ProductCategories.Any())
        {
            var laptop = context.Products.FirstOrDefault(p => p.Name == "Laptop");
            var novel = context.Products.FirstOrDefault(p => p.Name == "Novel");
            var electronics = context.Categorys.FirstOrDefault(c => c.Name == "Electronics");
            var books = context.Categorys.FirstOrDefault(c => c.Name == "Books");

            var productCategories = new List<ProductCategory>
            {
                new() { ProductId = laptop?.Id ?? 0, CategoryId = electronics?.Id ?? 0 }, // Laptop in Electronics
                new() { ProductId = novel?.Id ?? 0, CategoryId = books?.Id ?? 0 }         // Novel in Books
            };
            context.ProductCategories.AddRange(productCategories);
            context.SaveChanges();
        }
    }
}