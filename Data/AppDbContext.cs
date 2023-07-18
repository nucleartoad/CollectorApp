using Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Collection> Collections { get; set; }
		public DbSet<Item> Items { get; set; }
		public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
		{
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Collection>().HasMany(t => t.Items);
			modelBuilder.Entity<Item>();
		}
	}
}