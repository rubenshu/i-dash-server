using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class DeleteProductCommandHandler(ILogger logger, IUnitOfWork unitOfWork) : AsyncCommandHandlerBase<DeleteProductCommand, Result<bool>>(logger, unitOfWork), IDeleteProductCommandHandler
{
    protected override async Task<Result<bool>> DoHandle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await UnitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Product not found");
        await UnitOfWork.Products.DeleteAsync(entity, cancellationToken);
        // Commit handled by base
        return Result<bool>.Success(true);
    }
}