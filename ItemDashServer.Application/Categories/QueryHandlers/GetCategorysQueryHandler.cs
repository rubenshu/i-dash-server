using AutoMapper;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Handlers;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Categories.QueryHandlers;

public class GetCategoriesQueryHandler(ILogger logger, ICategoryRepository categoryRepository, IMapper mapper) : AsyncQueryHandlerBase<GetCategoriesQuery, Result<IEnumerable<CategoryDto>>>(logger), IGetCategoriesQueryHandler
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    protected override async Task<Result<IEnumerable<CategoryDto>>> DoExecute(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<CategoryDto>>.Success(_mapper.Map<IEnumerable<CategoryDto>>(categories));
    }
}