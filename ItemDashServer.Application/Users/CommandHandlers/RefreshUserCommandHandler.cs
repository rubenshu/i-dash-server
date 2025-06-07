using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;
using System.Security.Cryptography;
using ItemDashServer.Application.Users.Commands;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class RefreshUserCommandHandler(ApplicationDbContext dbContext, IAuthService authService, IMapper mapper) : IRequestHandler<RefreshUserCommand, (bool Success, string? Token, string? RefreshToken, UserDto? User)>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;

    public async Task<(bool Success, string? Token, string? RefreshToken, UserDto? User)> Handle(RefreshUserCommand request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(u => u.RefreshToken == request.RefreshToken);

        if (user == null || user.RefreshTokenExpiry == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            return (false, null, null, null);

        // Generate new JWT and refresh token
        var newJwt = _authService.GenerateJwtToken(user.Id, user.Username);
        var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var userDto = _mapper.Map<UserDto>(user);

        return (true, newJwt, newRefreshToken, userDto);
    }
}