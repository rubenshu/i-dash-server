using System.ComponentModel.DataAnnotations;

namespace ItemDashServer.Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public byte[] PasswordHash { get; set; } = default!;

    [Required]
    public byte[] PasswordSalt { get; set; } = default!;
}
