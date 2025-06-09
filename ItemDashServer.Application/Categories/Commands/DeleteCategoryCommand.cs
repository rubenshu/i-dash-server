using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.Commands;

public record DeleteCategoryCommand(int Id) : IRequest<Result<bool>>;
