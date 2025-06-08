using ItemDashServer.Application;
using ItemDashServer.Application.Categorys.Commands;
using ItemDashServer.Application.Categorys.Repositories;
using MediatR;

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _unitOfWork.Categories.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return true;
    }
}