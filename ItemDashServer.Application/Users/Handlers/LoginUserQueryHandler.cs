using MediatR;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;
using System.Text;
using ItemDashServer.Application.Users.Queries;

namespace ItemDashServer.Application.Users;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, (bool Success, UserDto? User)>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public LoginUserQueryHandler(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<(bool Success, UserDto? User)> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(u => u.Username == request.Username);
        if (user == null)
            return (false, null);

        using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        if (!computedHash.SequenceEqual(user.PasswordHash))
            return (false, null);

        var userDto = _mapper.Map<UserDto>(user);
        return (true, userDto);
    }
}