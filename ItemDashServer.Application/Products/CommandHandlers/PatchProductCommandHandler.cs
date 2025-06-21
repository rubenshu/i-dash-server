using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Domain.Entities;
using System.Text.Json.Nodes;
using System.Text.Json;
using ItemDashServer.Application.Common.Handlers;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class PatchProductCommandHandler(ILogger logger, IUnitOfWork unitOfWork) : AsyncCommandHandlerBase<PatchProductCommand, Result<bool>>(logger, unitOfWork), IPatchProductCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    protected override async Task<Result<bool>> DoHandle(PatchProductCommand request, CancellationToken cancellationToken)
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