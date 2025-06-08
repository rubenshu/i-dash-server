using MediatR;
using ItemDashServer.Application.Services;
using AutoMapper;
using System.Security.Cryptography;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class RefreshUserCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IMapper mapper) : IRequestHandler<RefreshUserCommand, (bool Success, string? Token, string? RefreshToken, UserDto? User)>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;

    public async Task<(bool Success, string? Token, string? RefreshToken, UserDto? User)> Handle(RefreshUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user == null || user.RefreshTokenExpiry == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            return (false, null, null, null);

        // Generate new JWT and refresh token
        var newJwt = _authService.GenerateJwtToken(user.Id, user.Username);
        var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        var userDto = _mapper.Map<UserDto>(user);

        return (true, newJwt, newRefreshToken, userDto);
    }
}