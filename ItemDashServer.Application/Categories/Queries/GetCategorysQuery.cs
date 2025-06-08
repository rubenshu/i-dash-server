using MediatR;
using ItemDashServer.Application.Categories;

namespace ItemDashServer.Application.Categories.Queries;
public record GetCategoriesQuery() : IRequest<IEnumerable<CategoryDto>>;
