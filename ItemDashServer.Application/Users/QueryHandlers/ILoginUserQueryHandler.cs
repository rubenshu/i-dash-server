using ItemDashServer.Application.Common;
using ItemDashServer.Application.Common.Abstractions;
using ItemDashServer.Application.Common.Results;
using ItemDashServer.Application.Users.Queries;

namespace ItemDashServer.Application.Users.QueryHandlers
{
    public interface ILoginUserQueryHandler : IAsyncQueryHandler<LoginUserQuery, Result<LoginResponseDto>> { }
}
