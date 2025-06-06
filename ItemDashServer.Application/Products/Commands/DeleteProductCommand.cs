using MediatR;

namespace ItemDashServer.Application.Products.Commands;

public record DeleteProductCommand(int Id) : IRequest<bool>;
