using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Queries;

namespace ItemDashServer.Application.Products.QueryHandlers
{
    public interface IGetProductsQueryHandler : IAsyncQueryHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>> { }
}
