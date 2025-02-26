// WSC/db-context.cs
using Microsoft.EntityFrameworkCore;
using WSC.Models;

namespace WSC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CollectionItem> CollectionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define unique constraints and indexes
            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardId)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Define relationship between CollectionItem and Card
            modelBuilder.Entity<CollectionItem>()
                .HasOne(ci => ci.Card)
                .WithMany()
                .HasForeignKey(ci => ci.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define indexes for collection items
            // Note: SQLite doesn't support filtered indexes, so we'll enforce uniqueness in application code
            modelBuilder.Entity<CollectionItem>()
                .HasIndex(ci => new { ci.UserId, ci.CardId });

            modelBuilder.Entity<CollectionItem>()
                .HasIndex(ci => new { ci.GuestId, ci.CardId });
        }
    }
}