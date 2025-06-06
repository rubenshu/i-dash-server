using MediatR;

namespace ItemDashServer.Application.Products.Queries;
public record GetProductsQuery() : IRequest<IEnumerable<ProductDto>>;
