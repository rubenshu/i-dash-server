using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Users.Commands;

public sealed record RefreshUserCommand(string RefreshToken) : ICommand;