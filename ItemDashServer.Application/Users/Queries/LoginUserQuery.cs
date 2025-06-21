using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Users.Queries;

public sealed record LoginUserQuery(
    string Username,
    string Password,
    string? RefreshToken = null
) : IQuery;