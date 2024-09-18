using InkSpaceWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace InkSpaceWeb.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) {
        
    }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fiction",DisplayOrder = 1},
            new Category { Id = 2, Name = "Autobiography",DisplayOrder = 2},
            new Category { Id = 3, Name = "SelfHelp",DisplayOrder = 3}
                );
    }
}