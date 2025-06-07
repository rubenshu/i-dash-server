using MediatR;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;
using System.Text;
using ItemDashServer.Application.Users.Queries;
using System.Security.Cryptography;

namespace ItemDashServer.Application.Users.QueryHandlers;

public class LoginUserQueryHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<LoginUserQuery, (bool Success, UserDto? User)>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<(bool Success, UserDto? User)> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(u => u.Username == request.Username);
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
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var userDto = _mapper.Map<UserDto>(user);
        return (true, userDto);
    }
}