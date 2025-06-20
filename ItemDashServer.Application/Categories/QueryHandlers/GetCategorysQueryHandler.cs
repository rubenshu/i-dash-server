using AutoMapper;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.QueryHandlers;

public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper) : IGetCategoriesQueryHandler
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<CategoryDto>>> ExecuteAsync(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<CategoryDto>>.Success(_mapper.Map<IEnumerable<CategoryDto>>(categories));
    }
}