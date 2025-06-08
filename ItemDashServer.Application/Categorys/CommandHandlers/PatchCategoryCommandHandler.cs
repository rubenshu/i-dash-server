using ItemDashServer.Application.Categorys.Commands;
using ItemDashServer.Application.Categorys.Repositories;
using ItemDashServer.Domain.Entities;
using MediatR;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class PatchCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<PatchCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<bool> Handle(PatchCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        var entityJson = JsonSerializer.Serialize(entity);
        var entityNode = JsonNode.Parse(entityJson)!;
        var patchNode = JsonNode.Parse(request.PatchDoc.RootElement.GetRawText())!;

        foreach (var prop in patchNode.AsObject())
        {
            entityNode[prop.Key] = prop.Value is null ? null : JsonNode.Parse(prop.Value.ToJsonString());
        }

        var patchedEntity = entityNode.Deserialize<Category>();
        if (patchedEntity == null) return false;

        entity.Name = patchedEntity.Name;
        entity.Description = patchedEntity.Description;
        entity.Price = patchedEntity.Price;

        await _categoryRepository.UpdateAsync(entity, cancellationToken);
        return true;
    }
}