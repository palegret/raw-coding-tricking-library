using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Models;

namespace TrickingLibrary.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Trick> Tricks { get; set; }
    public DbSet<TrickCategory> TrickCategories { get; set; }
    public DbSet<TrickRelationship> TrickRelationships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TrickCategory>()
            .HasKey(entity => new { entity.CategoryId, entity.TrickId });

        modelBuilder.Entity<TrickRelationship>()
            .HasKey(entity => new { entity.PrerequisiteId, entity.ProgressionId });

        modelBuilder.Entity<TrickRelationship>()
            .HasOne(entity => entity.Progression)
            .WithMany(entity => entity.Prerequisites)
            .HasForeignKey(entity => entity.ProgressionId);

        modelBuilder.Entity<TrickRelationship>()
            .HasOne(entity => entity.Prerequisite)
            .WithMany(entity => entity.Progressions)
            .HasForeignKey(entity => entity.PrerequisiteId);
    }
}
