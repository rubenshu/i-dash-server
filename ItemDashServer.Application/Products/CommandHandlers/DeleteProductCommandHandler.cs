using ItemDashServer.Application.Products.Commands;
using MediatR;
using ItemDashServer.Application.Products.Repositories;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        await _productRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }
}