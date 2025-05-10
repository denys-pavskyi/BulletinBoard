using BulletinBoard.WebClient.Models;

namespace BulletinBoard.WebClient.Data;

public class CategoryData
{
    public static List<CategoryDto> Categories = new()
    {
        new CategoryDto
        {
            Id = 1,
            Name = "Home Appliances",
            Subcategories = new List<SubcategoryDto>
            {
                new(1, "Refrigerators"),
                new(2, "Washing Machines"),
                new(3, "Water Heaters"),
                new(4, "Ovens"),
                new(5, "Hoods"),
                new(6, "Microwave Ovens")
            }
        },
        new CategoryDto
        {
            Id = 2,
            Name = "Computer Equipment",
            Subcategories = new List<SubcategoryDto>
            {
                new(7, "PCs"),
                new(8, "Laptops"),
                new(9, "Monitors"),
                new(10, "Printers"),
                new(11, "Scanners")
            }
        },
        new CategoryDto
        {
            Id = 3,
            Name = "Smartphones",
            Subcategories = new List<SubcategoryDto>
            {
                new(12, "Android Smartphones"),
                new(13, "iOS/Apple Smartphones")
            }
        },
        new CategoryDto
        {
            Id = 4,
            Name = "Others",
            Subcategories = new List<SubcategoryDto>
            {
                new(14, "Clothing"),
                new(15, "Footwear"),
                new(16, "Accessories"),
                new(17, "Sports Equipment"),
                new(18, "Toys")
            }
        }
    };
}