using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Domain.Entities;
using MediatR;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class PatchProductCommandHandler(IProductRepository productRepository) : IRequestHandler<PatchProductCommand, bool>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<bool> Handle(PatchProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        var entityJson = JsonSerializer.Serialize(entity);
        var entityNode = JsonNode.Parse(entityJson)!;
        var patchNode = JsonNode.Parse(request.PatchDoc.RootElement.GetRawText())!;

        foreach (var prop in patchNode.AsObject())
        {
            entityNode[prop.Key] = prop.Value is null ? null : JsonNode.Parse(prop.Value.ToJsonString());
        }

        var patchedEntity = entityNode.Deserialize<Product>();
        if (patchedEntity == null) return false;

        entity.Name = patchedEntity.Name;
        entity.Description = patchedEntity.Description;
        entity.Price = patchedEntity.Price;

        await _productRepository.UpdateAsync(entity, cancellationToken);
        return true;
    }
}