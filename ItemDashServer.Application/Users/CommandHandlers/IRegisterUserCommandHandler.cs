using ItemDashServer.Application.Common;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers
{
    public interface IRegisterUserCommandHandler
    {
        Task<Result<UserDto>> ExecuteAsync(RegisterUserCommand command, CancellationToken cancellationToken);
    }
}
