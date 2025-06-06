using MediatR;

namespace ItemDashServer.Application.Categorys.Commands;

public record UpdateCategoryCommand(int Id, string Name, string Description, decimal Price) : IRequest<bool>;
