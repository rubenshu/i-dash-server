using ItemDashServer.Application.Categories.Commands;
using MediatR;
using ItemDashServer.Application;
using ItemDashServer.Application.Categories.Repositories;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        await _unitOfWork.Categories.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return true;
    }
}