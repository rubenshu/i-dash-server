using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.Repositories;
using MediatR;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class UpdateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _productRepository.UpdateAsync(entity, cancellationToken);
        return true;
    }
}