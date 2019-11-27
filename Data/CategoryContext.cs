using Microsoft.EntityFrameworkCore;
using NTR02.Models;

namespace NTR02.Data
{
    public class CategoryContext : DbContext
    {
        public CategoryContext (DbContextOptions<CategoryContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<Category>()
                    .HasAlternateKey(p => p.Name);
                    
        }

        public DbSet<Category> Category { get; set; }
    }
}