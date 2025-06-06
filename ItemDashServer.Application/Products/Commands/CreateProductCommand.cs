using MediatR;

namespace ItemDashServer.Application.Products.Commands;

public record CreateProductCommand(string Name, string Description, decimal Price) : IRequest<ProductDto>;
