using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories.Commands;

namespace ItemDashServer.Application.Categories.CommandHandlers
{
    public interface IDeleteCategoryCommandHandler : IAsyncCommandHandler<DeleteCategoryCommand, Result<bool>> { }
}
