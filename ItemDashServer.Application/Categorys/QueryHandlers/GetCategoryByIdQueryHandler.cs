using AutoMapper;
using ItemDashServer.Application.Categorys.Queries;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Categorys.QueryHandlers;

public class GetCategoryByIdQueryHandler(ApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categorys
            .Include(c => c.ProductCategories)
                .ThenInclude(pc => pc.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null) return null;

        return _mapper.Map<CategoryDto>(entity);
    }
}