using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Products.Queries;

public record GetProductByIdQuery(int Id) : IQuery;
