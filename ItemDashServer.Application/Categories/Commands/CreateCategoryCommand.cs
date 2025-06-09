using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.Commands;

public record CreateCategoryCommand(string Name, string Description, decimal Price) : IRequest<Result<CategoryDto>>;
