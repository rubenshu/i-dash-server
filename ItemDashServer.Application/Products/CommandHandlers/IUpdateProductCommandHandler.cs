using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers
{
    public interface IUpdateProductCommandHandler : IAsyncCommandHandler<UpdateProductCommand, Result<bool>> { }
}
