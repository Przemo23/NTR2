using Microsoft.EntityFrameworkCore;
using NTR02.Models;

namespace NTR02.Data
{
    public class NotebookContext : DbContext
    {
        public NotebookContext (DbContextOptions<NotebookContext> options)
            : base(options)
        {
        }

        public DbSet<NoteCategory> NoteCategory { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Note> Note { get; set; }

        
    }
}