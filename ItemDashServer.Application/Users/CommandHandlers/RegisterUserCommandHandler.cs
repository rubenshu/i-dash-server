using MediatR;
using ItemDashServer.Domain.Entities;
using AutoMapper;
using ItemDashServer.Application.Users.Commands;
using ItemDashServer.Application;
using ItemDashServer.Application.Users.Repositories;

namespace ItemDashServer.Application.Users.CommandHandlers;

public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _unitOfWork.Users.GetByUsernameAsync(request.Username, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException("Username already exists.");

        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var user = new User
        {
            Username = request.Username,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };
        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<UserDto>(user);
    }
}