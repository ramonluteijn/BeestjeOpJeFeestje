using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeestjeOpJeFeestje.Repository;

public class MainContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public MainContext(DbContextOptions<MainContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .Property(u => u.Rank)
            .HasConversion<string>();

        CreateRelations(builder);
        SeedData(builder);
    }

    private static void CreateRelations(ModelBuilder builder)
    {
        // Define relationships here
    }

    private static void SeedData(ModelBuilder builder)
    {
        var user1 = new User
        {
            Id = 1,
            Rank = Rank.NONE,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            PhoneNumber = "0612345678",
            HouseNumber = "123",
            ZipCode = "1234AB",
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = "AQAAAAIAAYagAAAAEPS3HVgGwreh2VogbGYNNcFZeVOJgO8bLRs+04f5Iucpgy+P86IRXTI4/1xQcPFG2w=="
        };

        var user2 = new User
        {
            Id = 2,
            Rank = Rank.NONE,
            UserName = "customer",
            NormalizedUserName = "CUSTOMER",
            Email = "customer@example.com",
            NormalizedEmail = "CUSTOMER@EXAMPLE.COM",
            EmailConfirmed = true,
            PhoneNumber = "0612345678",
            HouseNumber = "123",
            ZipCode = "1234AB",
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = "AQAAAAIAAYagAAAAEPS3HVgGwreh2VogbGYNNcFZeVOJgO8bLRs+04f5Iucpgy+P86IRXTI4/1xQcPFG2w=="
        };

        builder.Entity<User>().HasData(user1, user2);

        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole<int> { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER" }
        );

        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int> { UserId = 1, RoleId = 1 },
            new IdentityUserRole<int> { UserId = 2, RoleId = 2 }
        );
    }
}