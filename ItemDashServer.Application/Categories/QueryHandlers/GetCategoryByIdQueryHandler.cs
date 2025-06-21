using AutoMapper;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Handlers;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Categories.QueryHandlers;

public class GetCategoryByIdQueryHandler(ILogger logger, ICategoryRepository categoryRepository, IMapper mapper) : AsyncQueryHandlerBase<GetCategoryByIdQuery, Result<CategoryDto>>(logger), IGetCategoryByIdQueryHandler
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    protected override async Task<Result<CategoryDto>> DoExecute(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken);
        if (entity == null) return Result<CategoryDto>.Failure("Category not found");
        return Result<CategoryDto>.Success(_mapper.Map<CategoryDto>(entity));
    }
}