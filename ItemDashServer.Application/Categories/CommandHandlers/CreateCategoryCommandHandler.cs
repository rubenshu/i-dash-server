using AutoMapper;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICreateCategoryCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CategoryDto>> ExecuteAsync(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        await _unitOfWork.Categories.AddAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync();
        return Result<CategoryDto>.Success(_mapper.Map<CategoryDto>(entity));
    }
}