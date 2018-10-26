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
	    public DbSet<ChannelOperations> ChannelOperations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelEvent>()
                .HasOne(p => p.ChannelUser)
                .WithMany(b => b.ChannelEvents);
                //.OnDelete(deleteBehavior: DeleteBehavior.Restrict);
	        
	        modelBuilder.Entity<ChannelOperations>()
		        .HasOne(p => p.ChannelUser)
		        .WithMany(b => b.ChannelOperations);
        }
    }

    
}
