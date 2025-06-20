using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Queries;

namespace ItemDashServer.Application.Products.QueryHandlers
{
    public interface IGetProductByIdQueryHandler
    {
        Task<Result<ProductDto>> ExecuteAsync(GetProductByIdQuery query, CancellationToken cancellationToken);
    }
}
