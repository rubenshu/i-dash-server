using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Domain.Entities;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class PatchProductCommandHandler(ApplicationDbContext context) : IRequestHandler<PatchProductCommand, bool>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Handle(PatchProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (entity == null) return false;

        var entityJson = JsonSerializer.Serialize(entity);
        var entityNode = JsonNode.Parse(entityJson)!;

        var patchNode = JsonNode.Parse(request.PatchDoc.RootElement.GetRawText())!;

        foreach (var prop in patchNode.AsObject())
        {
            entityNode[prop.Key] = prop.Value;
        }

        var patchedEntity = entityNode.Deserialize<Product>();
        if (patchedEntity == null) return false;

        entity.Name = patchedEntity.Name;
        entity.Description = patchedEntity.Description;
        entity.Price = patchedEntity.Price;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}