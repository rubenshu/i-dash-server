using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork) : IDeleteCategoryCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Category not found");

        await _unitOfWork.Categories.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}