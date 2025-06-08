using MediatR;

namespace ItemDashServer.Application.Categories.Queries;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>;
