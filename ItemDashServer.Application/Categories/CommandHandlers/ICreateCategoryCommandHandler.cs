using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Categories.CommandHandlers
{
    public interface ICreateCategoryCommandHandler : IAsyncCommandHandler<CreateCategoryCommand, Result<CategoryDto>> { }
}
