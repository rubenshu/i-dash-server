using MediatR;
using ItemDashServer.Application.Common;

namespace ItemDashServer.Application.Users.Queries;

public record GetUserByRefreshTokenQuery(string RefreshToken) : IRequest<Result<UserDto>>;