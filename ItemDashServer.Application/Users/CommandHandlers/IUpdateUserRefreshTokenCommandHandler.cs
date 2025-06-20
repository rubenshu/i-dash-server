using ItemDashServer.Application.Common;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers
{
    public interface IUpdateUserRefreshTokenCommandHandler
    {
        Task<Result<bool>> ExecuteAsync(UpdateUserRefreshTokenCommand command, CancellationToken cancellationToken);
    }
}
