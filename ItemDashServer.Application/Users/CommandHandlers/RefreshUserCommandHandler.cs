using ItemDashServer.Application.Services;
using AutoMapper;
using System.Security.Cryptography;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Common.Handlers;
using ItemDashServer.Application.Common.Results;
using ItemDashServer.Application.Common.Abstractions;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class RefreshUserResultDto
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public UserDto? User { get; set; }
}

public class RefreshUserCommandHandler(
    ILogger logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAuthService authService
) : AsyncCommandHandlerBase<RefreshUserCommand, Result<RefreshUserResultDto>>(logger, unitOfWork), IRefreshUserCommandHandler
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;

    protected override async Task<Result<RefreshUserResultDto>> DoHandle(RefreshUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user == null || user.RefreshTokenExpiry == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            return Result<RefreshUserResultDto>.Failure("Invalid or expired refresh token");

        // Generate new JWT and refresh token
        var newJwt = _authService.GenerateJwtToken(user.Id, user.Username, user.Role, user.Rights);
        var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();

        var userDto = _mapper.Map<UserDto>(user);
        var result = new RefreshUserResultDto
        {
            Token = newJwt,
            RefreshToken = newRefreshToken,
            User = userDto
        };
        return Result<RefreshUserResultDto>.Success(result);
    }
}