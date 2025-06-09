using MediatR;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Categories;

namespace ItemDashServer.Application.Categories.Queries;
public record GetCategoriesQuery() : IRequest<Result<IEnumerable<CategoryDto>>>;
