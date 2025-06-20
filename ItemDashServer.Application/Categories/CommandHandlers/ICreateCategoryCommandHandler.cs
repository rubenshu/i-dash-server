using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.Commands;

namespace ItemDashServer.Application.Categories.CommandHandlers
{
    public interface ICreateCategoryCommandHandler
    {
        Task<Result<CategoryDto>> ExecuteAsync(CreateCategoryCommand command, CancellationToken cancellationToken);
    }
}
