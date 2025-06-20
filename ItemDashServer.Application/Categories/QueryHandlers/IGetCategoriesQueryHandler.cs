using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.Queries;

namespace ItemDashServer.Application.Categories.QueryHandlers
{
    public interface IGetCategoriesQueryHandler
    {
        Task<Result<IEnumerable<CategoryDto>>> ExecuteAsync(GetCategoriesQuery query, CancellationToken cancellationToken);
    }
}
