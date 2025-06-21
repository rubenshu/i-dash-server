using ItemDashServer.Application.Common;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Application.Categories.Commands;

public record CreateCategoryCommand(
    int Id,
    string Name,
    string Description,
    decimal Price
) : ICommand;
