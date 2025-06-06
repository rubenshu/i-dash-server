using MediatR;

namespace ItemDashServer.Application.Categorys.Queries;
public record GetCategorysQuery() : IRequest<IEnumerable<CategoryDto>>;
