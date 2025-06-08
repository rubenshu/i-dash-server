using AutoMapper;
using ItemDashServer.Application.Categorys.Commands;
using ItemDashServer.Application.Categorys.Repositories;
using ItemDashServer.Domain.Entities;
using MediatR;

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        await _categoryRepository.AddAsync(entity, cancellationToken);
        return _mapper.Map<CategoryDto>(entity);
    }
}