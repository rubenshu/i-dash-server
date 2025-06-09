using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.Commands;

public record UpdateProductCommand(int Id, string Name, string Description, decimal Price) : IRequest<Result<bool>>;
