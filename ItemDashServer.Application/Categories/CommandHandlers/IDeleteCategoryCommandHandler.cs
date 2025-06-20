using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.Commands;

namespace ItemDashServer.Application.Categories.CommandHandlers
{
    public interface IDeleteCategoryCommandHandler
    {
        Task<Result<bool>> ExecuteAsync(DeleteCategoryCommand command, CancellationToken cancellationToken);
    }
}
