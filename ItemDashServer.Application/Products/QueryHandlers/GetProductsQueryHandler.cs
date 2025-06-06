using AutoMapper;
using ItemDashServer.Application.Products.Queries;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Products.QueryHandlers;

public class GetProductsQueryHandler(ApplicationDbContext context, IMapper mapper) : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}