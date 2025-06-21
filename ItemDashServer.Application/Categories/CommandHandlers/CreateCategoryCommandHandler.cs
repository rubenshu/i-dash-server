using AutoMapper;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Application.Categories.CommandHandlers;

public class CreateCategoryCommandHandler(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper) : AsyncCommandHandlerBase<CreateCategoryCommand, Result<CategoryDto>>(logger, unitOfWork), ICreateCategoryCommandHandler
{
    private readonly IMapper _mapper = mapper;

    protected override async Task<Result<CategoryDto>> DoHandle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };
        await UnitOfWork.Categories.AddAsync(entity, cancellationToken);
        // Commit handled by base
        return Result<CategoryDto>.Success(_mapper.Map<CategoryDto>(entity));
    }
}