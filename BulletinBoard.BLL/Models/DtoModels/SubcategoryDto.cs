﻿namespace BulletinBoard.BLL.Models.DtoModels;

public class SubcategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}