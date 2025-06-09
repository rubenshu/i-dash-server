using ItemDashServer.Application.Common;
using MediatR;
using System.Text.Json;

namespace ItemDashServer.Application.Products.Commands;

public record PatchProductCommand(int Id, JsonDocument PatchDoc) : IRequest<Result<bool>>;
