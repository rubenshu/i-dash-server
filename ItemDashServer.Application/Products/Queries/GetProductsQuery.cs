using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.Queries;
public record GetProductsQuery() : IRequest<Result<IEnumerable<ProductDto>>>;
