using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Handlers;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class UpdateCategoryCommandHandler(ILogger logger, IUnitOfWork unitOfWork) : AsyncCommandHandlerBase<UpdateCategoryCommand, Result<bool>>(logger, unitOfWork), IUpdateCategoryCommandHandler
{
    protected override async Task<Result<bool>> DoHandle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Category not found");
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;
        await UnitOfWork.Categories.UpdateAsync(entity, cancellationToken);
        // Commit handled by base
        return Result<bool>.Success(true);
    }
}