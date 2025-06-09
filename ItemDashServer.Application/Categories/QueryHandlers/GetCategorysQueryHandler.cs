using AutoMapper;
using ItemDashServer.Application.Categories.Queries;
using MediatR;
using ItemDashServer.Application.Categories.Repositories;
using AutoMapper.QueryableExtensions;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.QueryHandlers;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<GetCategoriesQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<CategoryDto>>.Success(_mapper.Map<IEnumerable<CategoryDto>>(categories));
    }
}