using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Categories.Queries;

public record GetCategoryByIdQuery(int Id) : IQuery;
