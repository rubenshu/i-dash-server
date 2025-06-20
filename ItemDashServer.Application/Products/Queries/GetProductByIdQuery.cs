using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.Queries;

public record GetProductByIdQuery(int Id) : IQuery;
