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
        public DbSet<ChannelEvent> ChannelEvent { get; set; }
        public DbSet<ChannelUser> ChannelUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelEvent>()
                .HasOne(p => p.ChannelUser)
                .WithMany(b => b.ChannelEvents);

            //modelBuilder.Entity<ChannelEvent>().ToTable("ChannelEvent");
            //modelBuilder.Entity<ChannelUser>().ToTable("ChannelUser");
        }
    }

    
}
