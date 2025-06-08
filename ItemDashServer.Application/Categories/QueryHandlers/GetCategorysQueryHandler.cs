using AutoMapper;
using ItemDashServer.Application.Categories.Queries;
using MediatR;
using ItemDashServer.Application.Categories.Repositories;
using AutoMapper.QueryableExtensions;

namespace ItemDashServer.Application.Categories.QueryHandlers;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}