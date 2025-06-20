using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.Queries;

public record GetCategoryByIdQuery(int Id) : IQuery;
