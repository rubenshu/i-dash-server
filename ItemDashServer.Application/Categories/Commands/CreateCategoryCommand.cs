using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Categories.Commands;

public record CreateCategoryCommand(
    int Id,
    string Name,
    string Description,
    decimal Price
) : ICommand;
