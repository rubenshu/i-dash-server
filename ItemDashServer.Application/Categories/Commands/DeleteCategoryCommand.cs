using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Categories.Commands;

public record DeleteCategoryCommand(int Id) : ICommand;
