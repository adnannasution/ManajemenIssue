using Microsoft.EntityFrameworkCore;
using NamaProyekAnda.Models;

namespace NamaProyekAnda.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Tindakan> Tindakan { get; set; }

    }
}
