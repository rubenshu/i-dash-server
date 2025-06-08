using AutoMapper;
using ItemDashServer.Application.Categorys.Queries;
using MediatR;
using ItemDashServer.Application.Categorys.Repositories;

namespace ItemDashServer.Application.Categorys.QueryHandlers;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return null;
        return _mapper.Map<CategoryDto>(entity);
    }
}