using ItemDashServer.Application.Categorys.Commands;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;     
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class DeleteCategoryCommandHandler(ApplicationDbContext context) : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categorys.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (entity == null) return false;

        _context.Categorys.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}