using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;
using ItemDashServer.Application.Users.Queries;

namespace ItemDashServer.Application.Users.QueryHandlers
{
    public interface IGetUserByRefreshTokenQueryHandler : IAsyncQueryHandler<GetUserByRefreshTokenQuery, Result<UserDto>> { }
}
