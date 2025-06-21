using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Products.Commands;

public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal Price
) : ICommand;
