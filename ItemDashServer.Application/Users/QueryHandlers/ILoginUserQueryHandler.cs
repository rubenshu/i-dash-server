using ItemDashServer.Application.Common;
using ItemDashServer.Application.Users.Queries;

namespace ItemDashServer.Application.Users.QueryHandlers
{
    public interface ILoginUserQueryHandler
    {
        Task<Result<LoginResponseDto>> ExecuteAsync(LoginUserQuery query, CancellationToken cancellationToken);
    }
}
