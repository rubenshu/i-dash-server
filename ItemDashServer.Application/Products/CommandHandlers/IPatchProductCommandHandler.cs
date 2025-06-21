using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers
{
    public interface IPatchProductCommandHandler : IAsyncCommandHandler<PatchProductCommand, Result<bool>> { }
}
