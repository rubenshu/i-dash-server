using MediatR;
using System.Text.Json;

namespace ItemDashServer.Application.Categorys.Commands;

public record PatchCategoryCommand(int Id, JsonDocument PatchDoc) : IRequest<bool>;
