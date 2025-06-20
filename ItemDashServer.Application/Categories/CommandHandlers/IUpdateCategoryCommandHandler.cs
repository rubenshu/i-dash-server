using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.Commands;

namespace ItemDashServer.Application.Categories.CommandHandlers
{
    public interface IUpdateCategoryCommandHandler
    {
        Task<Result<bool>> ExecuteAsync(UpdateCategoryCommand command, CancellationToken cancellationToken);
    }
}
