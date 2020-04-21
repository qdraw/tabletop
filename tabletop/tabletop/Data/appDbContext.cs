using Microsoft.EntityFrameworkCore;
using tabletop.Models;

namespace tabletop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ChannelEvent> ChannelEvent { get; set; }
        public DbSet<ChannelUser> ChannelUser { get; set; }

        public DbSet<ChannelActivity> ChannelActivity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
	        // needed for mysql:
	        modelBuilder.Entity<ChannelUser>(entity => entity.Property(m => m.NameId).HasMaxLength(80));
	        modelBuilder.Entity<ChannelEvent>()
                .HasOne(p => p.ChannelUser)
                .WithMany(b => b.ChannelEvents);
	        
	        modelBuilder.Entity<ChannelActivity>()
		        .HasOne(p => p.ChannelUser)
		        .WithMany(b => b.ChannelActivities);
        }
    }

    
}
