using Microsoft.EntityFrameworkCore;
using tabletop.Models;

namespace tabletop.Data
{
    public class AppDbContext : DbContext
    {
        //// Contructor for testing
        //public AppDbContext()
        //{
        //}

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UpdateStatus> UpdateStatus { get; set; }
        public DbSet<Channel> Channel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<UpdateStatus>()
            //    .HasMany<Channel>(g => g.Channels)
            //    .WithOne(s => s.UpdateStatus)
            //    .HasForeignKey(s => s.NameId)
            //    .OnDelete(DeleteBehavior.Restrict);


        }
    }

    
}
