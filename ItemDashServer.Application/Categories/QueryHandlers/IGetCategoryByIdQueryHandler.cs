using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.Queries;

namespace ItemDashServer.Application.Categories.QueryHandlers
{
    public interface IGetCategoryByIdQueryHandler
    {
        Task<Result<CategoryDto>> ExecuteAsync(GetCategoryByIdQuery query, CancellationToken cancellationToken);
    }
}
