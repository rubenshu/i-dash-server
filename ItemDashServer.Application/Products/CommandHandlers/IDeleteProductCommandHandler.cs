using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers
{
    public interface IDeleteProductCommandHandler : IAsyncCommandHandler<DeleteProductCommand, Result<bool>> { }
}
