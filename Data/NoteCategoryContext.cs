using Microsoft.EntityFrameworkCore;
using NTR02.Models;

namespace NTR02.Data
{
    public class NoteCategoryContext : DbContext
    {
        public NoteCategoryContext (DbContextOptions<NoteCategoryContext> options)
            : base(options)
        {
        }

        public DbSet<NoteCategory> NoteCategory { get; set; }
    }
}