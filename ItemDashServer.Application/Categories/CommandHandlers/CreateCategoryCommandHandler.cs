using AutoMapper;
using ItemDashServer.Application;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Domain.Entities;
using MediatR;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        await _unitOfWork.Categories.AddAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<CategoryDto>(entity);
    }
}