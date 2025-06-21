using ItemDashServer.Application.Users.Repositories;
using AutoMapper;
using System.Text;
using ItemDashServer.Application.Users.Queries;
using System.Security.Cryptography;
using ItemDashServer.Application.Common;
using ItemDashServer.Application.Services;
using ItemDashServer.Application.Common.Results;

namespace ItemDashServer.Application.Users.QueryHandlers;

public class LoginUserQueryHandler(
    IUserRepository userRepository,
    IMapper mapper,
    ILoginRateLimiter rateLimiter,
    IAuthService authService) : ILoginUserQueryHandler
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoginRateLimiter _rateLimiter = rateLimiter;
    private readonly IAuthService _authService = authService;

    public async Task<Result<LoginResponseDto>> ExecuteAsync(LoginUserQuery request, CancellationToken cancellationToken)
    {
        if (!await _rateLimiter.AllowAttemptAsync(request.Username))
            return Result<LoginResponseDto>.Failure("Too many login attempts. Please try again later.");

        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null)
        {
            await _rateLimiter.RegisterFailureAsync(request.Username);
            return Result<LoginResponseDto>.Failure("User not found");
        }

        var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        if (!computedHash.SequenceEqual(user.PasswordHash))
        {
            await _rateLimiter.RegisterFailureAsync(request.Username);
            return Result<LoginResponseDto>.Failure("Invalid credentials");
        }

        await _rateLimiter.ResetFailuresAsync(request.Username);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateAsync(user, cancellationToken);

        var userDto = _mapper.Map<UserDto>(user);
        var token = _authService.GenerateJwtToken(userDto.Id, userDto.Username, userDto.Role, userDto.Rights);

        var response = new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            User = userDto
        };
        return Result<LoginResponseDto>.Success(response);
    }
}