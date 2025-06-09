using ItemDashServer.Application.Categories.Commands;
using MediatR;
using ItemDashServer.Application;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Category not found");

        await _unitOfWork.Categories.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}