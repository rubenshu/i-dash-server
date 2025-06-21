using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Products.Commands;

public record DeleteProductCommand(int Id) : ICommand;
