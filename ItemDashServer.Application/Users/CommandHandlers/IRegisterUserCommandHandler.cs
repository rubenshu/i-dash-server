using ItemDashServer.Application.Common;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers
{
    public interface IRegisterUserCommandHandler : IAsyncCommandHandler<RegisterUserCommand, Result<UserDto>> { }
}
