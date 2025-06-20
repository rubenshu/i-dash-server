using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.Queries;
public record GetCategoriesQuery() : IRequest<Result<IEnumerable<CategoryDto>>>;
