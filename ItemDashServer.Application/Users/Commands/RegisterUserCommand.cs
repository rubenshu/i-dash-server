using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Commands;

public sealed record RegisterUserCommand(string Username, string Password) : ICommand;