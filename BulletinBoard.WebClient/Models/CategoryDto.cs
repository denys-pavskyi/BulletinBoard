namespace BulletinBoard.WebClient.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<SubcategoryDto> Subcategories { get; set; } = new();

    public CategoryDto() { }
    public CategoryDto(int id, string name) => (Id, Name) = (id, name);
}

public record SubcategoryDto(int Id, string Name);