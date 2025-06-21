using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Categories.Commands;

public record DeleteCategoryCommand(int Id) : ICommand;
