using ItemDashServer.Application.Common;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.Repositories;
using ItemDashServer.Domain.Entities;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class PatchProductCommandHandler(IUnitOfWork unitOfWork) : IPatchProductCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(PatchProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Product not found");

        var entityJson = JsonSerializer.Serialize(entity);
        var entityNode = JsonNode.Parse(entityJson)!;
        var patchNode = JsonNode.Parse(request.PatchDoc.RootElement.GetRawText())!;

        foreach (var prop in patchNode.AsObject())
        {
            entityNode[prop.Key] = prop.Value is null ? null : JsonNode.Parse(prop.Value.ToJsonString());
        }

        var patchedEntity = entityNode.Deserialize<Product>();
        if (patchedEntity == null)
            return Result<bool>.Failure("Patch failed: deserialization error");

        entity.Name = patchedEntity.Name;
        entity.Description = patchedEntity.Description;
        entity.Price = patchedEntity.Price;

        await _unitOfWork.Products.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}