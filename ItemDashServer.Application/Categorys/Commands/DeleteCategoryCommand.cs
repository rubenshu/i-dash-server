using MediatR;

namespace ItemDashServer.Application.Categorys.Commands;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;
