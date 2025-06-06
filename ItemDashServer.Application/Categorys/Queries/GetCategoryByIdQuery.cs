using MediatR;

namespace ItemDashServer.Application.Categorys.Queries;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>;
