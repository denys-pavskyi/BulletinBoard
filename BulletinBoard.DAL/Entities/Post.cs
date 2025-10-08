using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoard.DAL.Entities;

public class Post
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(3) ,MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int SubcategoryId { get; set; }

    [ForeignKey(nameof(SubcategoryId))]
    public Subcategory SubCategory { get; set; } = null!;

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

}