using MediatR;
using ItemDashServer.Application.Users.Repositories;
using AutoMapper;
using System.Text;
using ItemDashServer.Application.Users.Queries;
using System.Security.Cryptography;

namespace ItemDashServer.Application.Users.QueryHandlers;

public class LoginUserQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<LoginUserQuery, (bool Success, UserDto? User)>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<(bool Success, UserDto? User)> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null)
            return (false, null);

        var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        if (!computedHash.SequenceEqual(user.PasswordHash))
            return (false, null);

        // If a refresh token is provided in the request, update it
        if (!string.IsNullOrEmpty(request.RefreshToken))
        {
            user.RefreshToken = request.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        var userDto = _mapper.Map<UserDto>(user);
        return (true, userDto);
    }
}