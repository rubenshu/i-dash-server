using MediatR;

namespace ItemDashServer.Application.Categories.Commands;

public record UpdateCategoryCommand(int Id, string Name, string Description, decimal Price) : IRequest<bool>;
