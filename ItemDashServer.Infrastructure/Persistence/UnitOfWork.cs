using System;
using System.Threading.Tasks;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Infrastructure.Persistence
{
    public class UnitOfWork(
        ApplicationDbContext context,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IUserRepository userRepository) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        public ICategoryRepository Categories { get; } = categoryRepository;
        public IProductRepository Products { get; } = productRepository;
        public IUserRepository Users { get; } = userRepository;

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
