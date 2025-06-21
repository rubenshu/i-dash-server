using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Application.Users.Repositories;

namespace ItemDashServer.Application.Common.Abstractions;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    IUserRepository Users { get; }
    Task<int> CommitAsync();
    void Rollback();
}
