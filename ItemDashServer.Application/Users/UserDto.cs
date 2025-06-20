namespace ItemDashServer.Application.Users;

public class UserDto
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string Role { get; set; }
    public List<string> Rights { get; set; } = [];
}