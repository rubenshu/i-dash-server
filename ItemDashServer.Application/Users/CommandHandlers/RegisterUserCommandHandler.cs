using MediatR;
using ItemDashServer.Domain.Entities;
using ItemDashServer.Infrastructure.Persistence;
using AutoMapper;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application.Users.Repositories;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException("Username already exists.");

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var user = new User
        {
            Username = request.Username,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };
        await _userRepository.AddAsync(user, cancellationToken);
        return _mapper.Map<UserDto>(user);
    }
}