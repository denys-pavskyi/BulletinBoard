using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.DAL.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(60)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? PasswordHash { get; set; } // null if google

    [Required]
    [MaxLength(20)]
    public string Provider { get; set; } = "Local"; // "Local"/"Google"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //Relationships
    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}