using E_Commerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.IdentityData.DbContexts;

public class StoreIdentityDbContext : IdentityDbContext<ApplicationUser>
{

    public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<ApplicationUser>().OwnsMany(x => x.RefreshTokens, a =>
        {
            a.HasIndex(t => t.Token).IsUnique();
        });
        builder.Entity<Address>().ToTable("Addresses");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
    }
}