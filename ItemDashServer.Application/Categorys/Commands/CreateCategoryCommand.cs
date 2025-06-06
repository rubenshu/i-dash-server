using MediatR;

namespace ItemDashServer.Application.Categorys.Commands;

public record CreateCategoryCommand(string Name, string Description, decimal Price) : IRequest<CategoryDto>;
