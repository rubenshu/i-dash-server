using MediatR;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;
using System.Text;
using ItemDashServer.Application.Users.Queries;

namespace ItemDashServer.Application.Users.QueryHandlers;

public class LoginUserQueryHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<LoginUserQuery, (bool Success, UserDto? User)>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public Task<(bool Success, UserDto? User)> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.SingleOrDefault(u => u.Username == request.Username);
        if (user == null)
            return Task.FromResult<(bool Success, UserDto? User)>((false, null));

        using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        if (!computedHash.SequenceEqual(user.PasswordHash))
            return Task.FromResult<(bool Success, UserDto? User)>((false, null));

        var userDto = _mapper.Map<UserDto>(user);
        return Task.FromResult<(bool Success, UserDto? User)>((true, userDto));
    }
}