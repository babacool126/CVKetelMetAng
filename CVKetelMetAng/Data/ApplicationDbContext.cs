namespace CVKetelMetAng.Data;
using CVKetelMetAng.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Klant> Klanten { get; set; }
    public DbSet<Afspraak> Afspraken { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Fluent API configuraties indien nodig
    }
}

