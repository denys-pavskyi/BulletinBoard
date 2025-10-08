using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.DAL.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;


    //Relationship with Subcategory many(Subcategory) - to - one(Category)
    public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
}