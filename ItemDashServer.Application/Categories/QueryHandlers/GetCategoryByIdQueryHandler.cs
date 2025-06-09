using AutoMapper;
using ItemDashServer.Application.Categories.Queries;
using MediatR;
using ItemDashServer.Application.Categories.Repositories;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.QueryHandlers;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return Result<CategoryDto>.Failure("Category not found");
        return Result<CategoryDto>.Success(_mapper.Map<CategoryDto>(entity));
    }
}