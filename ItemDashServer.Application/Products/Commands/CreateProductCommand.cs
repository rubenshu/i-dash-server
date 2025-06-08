using ItemDashServer.Application.Categorys;
using MediatR;

namespace ItemDashServer.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    IEnumerable<int>? CategoryIds // Accept a list of category IDs
) : IRequest<ProductDto>;
