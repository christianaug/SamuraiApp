using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
	public class SamuraiContext : DbContext
	{
		public DbSet<Samurai> Samurais { get; set; }
		public DbSet<Quote> Quotes { get; set; }
		public DbSet<Clan> Clans { get; set; }
		public DbSet<Battle> Battles { get; set; }

		public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
		{
			//because every time a new constructor and api endpoint is called, the context shouldnt waste its resources on setting up tracking
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		//gets called internally at run time when efcore is working out the datamodel
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//tells the model builder that the entity SamuraiBattle has a key that consists of
			//both the samuraiID and the battleID, they keys are also foreign keys that point to the the actual entries in either table
			//this is the final step needed to create a many to many
			modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
			//tells the model builder that the Horse entity will map to a table called Horses
			modelBuilder.Entity<Horse>().ToTable("Horses");
			modelBuilder.Entity<SecretIdentity>().ToTable("SecretIdentities");
			//modelBuilder.Entity<SecretIdentity>().ToTable("SecretIdentities");
			//samurai "Has One" Secre Identity Property and, that property has a Samurai Property that is required
			//modelBuilder.Entity<Samurai>()
			//	.HasOne(s => s.SecretIdentity)
			//	.WithOne(i => i.Samurai).IsRequired();
		}
	}
}
