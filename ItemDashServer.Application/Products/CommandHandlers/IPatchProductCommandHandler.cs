using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers
{
    public interface IPatchProductCommandHandler
    {
        Task<Result<bool>> ExecuteAsync(PatchProductCommand command, CancellationToken cancellationToken);
    }
}
