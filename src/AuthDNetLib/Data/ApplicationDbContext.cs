using AuthDNetLib.Models.Tokens;
using AuthDNetLib.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace AuthDNetLib.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TUser> Users => Set<TUser>();
    public DbSet<Token> Tokens => Set<Token>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasIndex(u => u.Id).IsUnique();
            entity.HasIndex(u => u.Name).IsUnique();
            entity.HasIndex(u => u.Login).IsUnique();
            entity.HasIndex(u => u.Password).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.CellPhone).IsUnique();
            entity.HasIndex(u => u.AuthToken).IsUnique();
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasIndex(u => u.Id).IsUnique();
            entity.HasIndex(u => u.SessionToken).IsUnique();
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}