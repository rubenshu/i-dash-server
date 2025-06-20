using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Commands;

public sealed record RefreshUserCommand(string RefreshToken) : ICommand;