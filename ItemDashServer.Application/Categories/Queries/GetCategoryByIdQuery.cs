using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.Queries;

public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryDto>>;
