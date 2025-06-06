using ItemDashServer.Application.Products.Commands;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;     
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Products.CommandHandlers;

public class DeleteProductCommandHandler(ApplicationDbContext context) : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (entity == null) return false;

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}