using ItemDashServer.Application;
using ItemDashServer.Application.Products.Commands;
using MediatR;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class UpdateProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _unitOfWork.Products.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return true;
    }
}