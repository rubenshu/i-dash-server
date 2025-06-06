using ItemDashServer.Application.Categorys.Commands;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class UpdateCategoryCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categorys.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (entity == null) return false;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}