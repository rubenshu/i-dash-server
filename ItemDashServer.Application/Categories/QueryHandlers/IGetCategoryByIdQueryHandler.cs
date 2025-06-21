using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Categories.QueryHandlers
{
    public interface IGetCategoryByIdQueryHandler : IAsyncQueryHandler<GetCategoryByIdQuery, Result<CategoryDto>> { }
}
