using System.Text.Json;
using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Products.Commands;

public record PatchProductCommand(int Id, JsonDocument PatchDoc) : ICommand;
