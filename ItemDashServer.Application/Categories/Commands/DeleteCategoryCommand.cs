using MediatR;

namespace ItemDashServer.Application.Categories.Commands;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;
