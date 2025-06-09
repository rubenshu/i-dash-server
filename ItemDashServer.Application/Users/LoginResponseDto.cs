namespace ItemDashServer.Application.Users;

public class LoginResponseDto
{
    public required string Token { get; set; }
    public string? RefreshToken { get; set; }
    public required UserDto User { get; set; }
}