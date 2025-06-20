using AutoMapper;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.QueryHandlers;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper) : IGetCategoryByIdQueryHandler
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CategoryDto>> ExecuteAsync(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken);
        if (entity == null) return Result<CategoryDto>.Failure("Category not found");
        return Result<CategoryDto>.Success(_mapper.Map<CategoryDto>(entity));
    }
}