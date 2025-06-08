using AutoMapper;
using ItemDashServer.Application.Categorys.Queries;
using MediatR;
using ItemDashServer.Application.Categorys.Repositories;
using AutoMapper.QueryableExtensions;

namespace ItemDashServer.Application.Categorys.QueryHandlers;

public class GetCategorysQueryHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<GetCategorysQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategorysQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}