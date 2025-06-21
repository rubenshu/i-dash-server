using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Users.Commands;

public sealed record RegisterUserCommand(string Username, string Password) : ICommand;