using ItemDashServer.Application.Categorys.Commands;
using ItemDashServer.Application.Categorys.Repositories;
using MediatR;

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Price = request.Price;

        await _categoryRepository.UpdateAsync(entity, cancellationToken);
        return true;
    }
}