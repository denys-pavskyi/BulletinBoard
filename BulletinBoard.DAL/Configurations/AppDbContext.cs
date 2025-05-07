using BulletinBoard.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.Configurations;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Subcategory> Subcategories { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Category>().HasData(InitialData.Categories);
        modelBuilder.Entity<Subcategory>().HasData(InitialData.Subcategories.ToArray());

    }
}