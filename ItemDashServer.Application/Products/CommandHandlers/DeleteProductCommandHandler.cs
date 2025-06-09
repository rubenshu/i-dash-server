using ItemDashServer.Application;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;
using MediatR;
using ItemDashServer.Application.Products.Repositories;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class DeleteProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Product not found");

        await _unitOfWork.Products.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}