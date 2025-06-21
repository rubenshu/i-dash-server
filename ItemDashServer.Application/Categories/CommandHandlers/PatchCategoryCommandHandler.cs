using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Domain.Entities;
using System.Text.Json.Nodes;
using System.Text.Json;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class PatchCategoryCommandHandler(ICategoryRepository categoryRepository)
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result<bool>> ExecuteAsync(PatchCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<bool>.Failure("Category not found");

        var entityJson = JsonSerializer.Serialize(entity);
        var entityNode = JsonNode.Parse(entityJson)!;
        var patchNode = JsonNode.Parse(request.PatchDoc.RootElement.GetRawText())!;

        foreach (var prop in patchNode.AsObject())
        {
            entityNode[prop.Key] = prop.Value is null ? null : JsonNode.Parse(prop.Value.ToJsonString());
        }

        var patchedEntity = entityNode.Deserialize<Category>();
        if (patchedEntity == null) return Result<bool>.Failure("Patch failed");

        entity.Name = patchedEntity.Name;
        entity.Description = patchedEntity.Description;
        entity.Price = patchedEntity.Price;

        await _categoryRepository.UpdateAsync(entity, cancellationToken);
        return Result<bool>.Success(true);
    }
}