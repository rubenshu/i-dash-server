using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers
{
    public interface IUpdateProductCommandHandler
    {
        Task<Result<bool>> ExecuteAsync(UpdateProductCommand command, CancellationToken cancellationToken);
    }
}
