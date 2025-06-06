using AutoMapper;
using ItemDashServer.Application.Categorys.Queries;
using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ItemDashServer.Application.Categorys.QueryHandlers;

public class GetCategorysQueryHandler(ApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCategorysQuery, IEnumerable<CategoryDto>>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategorysQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categorys
            .AsNoTracking()
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}