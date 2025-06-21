using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;
using ItemDashServer.Application.Products.Queries;

namespace ItemDashServer.Application.Products.QueryHandlers
{
    public interface IGetProductsQueryHandler : IAsyncQueryHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>> { }
}
