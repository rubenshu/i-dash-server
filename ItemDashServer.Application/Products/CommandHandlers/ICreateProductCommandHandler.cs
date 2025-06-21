using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers
{
    public interface ICreateProductCommandHandler : IAsyncCommandHandler<CreateProductCommand, Result<ProductDto>> { }
}
