using ItemDashServer.Application.Categorys.Commands;
using MediatR;
using ItemDashServer.Application.Categorys.Repositories;

namespace ItemDashServer.Application.Categorys.CommandHandlers;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        await _categoryRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }
}