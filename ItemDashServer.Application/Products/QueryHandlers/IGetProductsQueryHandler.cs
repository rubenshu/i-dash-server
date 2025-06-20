using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Queries;

namespace ItemDashServer.Application.Products.QueryHandlers
{
    public interface IGetProductsQueryHandler
    {
        Task<Result<IEnumerable<ProductDto>>> ExecuteAsync(GetProductsQuery query, CancellationToken cancellationToken);
    }
}
