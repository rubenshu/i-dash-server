using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Users.Queries;

public sealed record GetUserByRefreshTokenQuery(string RefreshToken) : IQuery;