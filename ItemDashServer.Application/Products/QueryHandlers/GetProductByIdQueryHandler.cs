using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductByIdQueryHandler(ApplicationDbContext context, IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null) return null;

        return _mapper.Map<ProductDto>(entity);
    }
}