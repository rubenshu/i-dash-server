using MediatR;
using ItemDashServer.Application.Users.Repositories;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class UpdateUserRefreshTokenCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserRefreshTokenCommand>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task Handle(UpdateUserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user != null)
        {
            user.RefreshToken = request.RefreshToken;
            user.RefreshTokenExpiry = request.RefreshTokenExpiry;
            await _userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}