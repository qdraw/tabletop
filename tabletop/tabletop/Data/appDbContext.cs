using Microsoft.EntityFrameworkCore;
using tabletop.Models;

namespace tabletop.Data
{
    public class AppDbContext : DbContext
    {
        // Contructor for testing
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UpdateStatus> UpdateStatus { get; set; }
    }
}
