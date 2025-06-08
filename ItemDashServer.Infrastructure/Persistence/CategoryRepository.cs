using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ItemDashServer.Application.Categories.Repositories;

namespace ItemDashServer.Infrastructure.Persistence;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Categories
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Categories
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .ToListAsync(cancellationToken);

    public Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Add(category);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(category);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Remove(category);
        return Task.CompletedTask;
    }
}
