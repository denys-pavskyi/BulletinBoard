using BulletinBoard.DAL.Entities;

namespace BulletinBoard.DAL.Configurations;

public static class InitialData
{
    public static List<Category> Categories =
    [
        new Category { Id = 1, Name = "Home Appliances" },
        new Category { Id = 2, Name = "Computer Equipment" },
        new Category { Id = 3, Name = "Smartphones" },
        new Category { Id = 4, Name = "Others" }
    ];


    public static List<Subcategory> Subcategories =
    [
        // Home Appliances
        new Subcategory { Id = 1, Name = "Refrigerators", CategoryId = 1 },
        new Subcategory { Id = 2, Name = "Washing Machines", CategoryId = 1 },
        new Subcategory { Id = 3, Name = "Water Heaters", CategoryId = 1 },
        new Subcategory { Id = 4, Name = "Ovens", CategoryId = 1 },
        new Subcategory { Id = 5, Name = "Hoods", CategoryId = 1 },
        new Subcategory { Id = 6, Name = "Microwave Ovens", CategoryId = 1 },

        // Computer Equipment
        new Subcategory { Id = 7, Name = "PCs", CategoryId = 2 },
        new Subcategory { Id = 8, Name = "Laptops", CategoryId = 2 },
        new Subcategory { Id = 9, Name = "Monitors", CategoryId = 2 },
        new Subcategory { Id = 10, Name = "Printers", CategoryId = 2 },
        new Subcategory { Id = 11, Name = "Scanners", CategoryId = 2 },

        // Smartphones
        new Subcategory { Id = 12, Name = "Android Smartphones", CategoryId = 3 },
        new Subcategory { Id = 13, Name = "iOS/Apple Smartphones", CategoryId = 3 },

        // Others
        new Subcategory { Id = 14, Name = "Clothing", CategoryId = 4 },
        new Subcategory { Id = 15, Name = "Footwear", CategoryId = 4 },
        new Subcategory { Id = 16, Name = "Accessories", CategoryId = 4 },
        new Subcategory { Id = 17, Name = "Sports Equipment", CategoryId = 4 },
        new Subcategory { Id = 18, Name = "Toys", CategoryId = 4 }
    ];

}