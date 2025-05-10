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

    [Required]
    [MaxLength(40)]
    public string Password { get; set; } = string.Empty;

    //Relationship with Posts many(Post) - to - one(User)
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}