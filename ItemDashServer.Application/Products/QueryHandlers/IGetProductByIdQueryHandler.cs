using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Queries;

namespace ItemDashServer.Application.Products.QueryHandlers
{
    public interface IGetProductByIdQueryHandler : IAsyncQueryHandler<GetProductByIdQuery, Result<ProductDto>> { }
}
