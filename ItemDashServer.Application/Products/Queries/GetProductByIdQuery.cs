using MediatR;

namespace ItemDashServer.Application.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
