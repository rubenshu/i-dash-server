using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Queries;

public sealed record GetUserByRefreshTokenQuery(string RefreshToken) : IQuery;