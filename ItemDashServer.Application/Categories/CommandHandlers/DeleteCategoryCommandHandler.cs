using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class DeleteCategoryCommandHandler(ILogger logger, IUnitOfWork unitOfWork) : AsyncCommandHandlerBase<DeleteCategoryCommand, Result<bool>>(logger, unitOfWork), IDeleteCategoryCommandHandler
{
    protected override async Task<Result<bool>> DoHandle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Category not found");
        await UnitOfWork.Categories.DeleteAsync(entity, cancellationToken);
        // Commit handled by base
        return Result<bool>.Success(true);
    }
}