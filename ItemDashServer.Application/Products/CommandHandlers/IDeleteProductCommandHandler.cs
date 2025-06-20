using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers
{
    public interface IDeleteProductCommandHandler
    {
        Task<Result<bool>> ExecuteAsync(DeleteProductCommand command, CancellationToken cancellationToken);
    }
}
