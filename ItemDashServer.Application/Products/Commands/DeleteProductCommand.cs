using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Products.Commands;

public record DeleteProductCommand(int Id) : ICommand;
