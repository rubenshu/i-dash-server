using ItemDashServer.Application.Common;
using ItemDashServer.Application.Users.Queries;

namespace ItemDashServer.Application.Users.QueryHandlers
{
    public interface IGetUserByRefreshTokenQueryHandler
    {
        Task<Result<UserDto>> ExecuteAsync(GetUserByRefreshTokenQuery query, CancellationToken cancellationToken);
    }
}
