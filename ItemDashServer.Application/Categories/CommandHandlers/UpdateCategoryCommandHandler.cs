using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork) : IUpdateCategoryCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Category not found");

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _unitOfWork.Categories.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}