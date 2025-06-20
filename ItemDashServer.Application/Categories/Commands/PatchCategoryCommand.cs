using System.Text.Json;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.Commands;

public record PatchCategoryCommand(int Id, JsonDocument PatchDoc) : ICommand;
