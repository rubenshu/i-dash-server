using System.Text.Json;
using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Categories.Commands;

public record PatchCategoryCommand(int Id, JsonDocument PatchDoc) : ICommand;
