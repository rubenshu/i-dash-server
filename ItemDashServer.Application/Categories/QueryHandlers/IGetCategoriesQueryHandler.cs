using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.Queries;

namespace ItemDashServer.Application.Categories.QueryHandlers
{
    public interface IGetCategoriesQueryHandler : IAsyncQueryHandler<GetCategoriesQuery, Result<IEnumerable<CategoryDto>>> { }
}
