using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class UpdateProductCommandHandler(ILogger logger, IUnitOfWork unitOfWork) : AsyncCommandHandlerBase<UpdateProductCommand, Result<bool>>(logger, unitOfWork), IUpdateProductCommandHandler
{
    protected override async Task<Result<bool>> DoHandle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Product not found");
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;
        await UnitOfWork.Products.UpdateAsync(entity, cancellationToken);
        // Commit handled by base
        return Result<bool>.Success(true);
    }
}