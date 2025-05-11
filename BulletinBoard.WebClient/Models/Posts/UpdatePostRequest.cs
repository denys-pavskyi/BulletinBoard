﻿namespace BulletinBoard.WebClient.Models.Posts;

public class UpdatePostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int SubcategoryId { get; set; }
}