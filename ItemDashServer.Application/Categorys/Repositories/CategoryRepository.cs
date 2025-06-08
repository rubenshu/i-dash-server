using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ItemDashServer.Application.Categorys.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Categorys
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Categorys
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categorys.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categorys.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categorys.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
