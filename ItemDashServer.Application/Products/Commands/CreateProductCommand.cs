using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    IEnumerable<int>? CategoryIds
) : ICommand;
