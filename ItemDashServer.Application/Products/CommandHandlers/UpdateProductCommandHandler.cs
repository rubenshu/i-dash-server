using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class UpdateProductCommandHandler(IUnitOfWork unitOfWork) : IUpdateProductCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Product not found");

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _unitOfWork.Products.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}