using ItemDashServer.Application.Products.Commands;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class UpdateProductCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (entity == null) return false;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}