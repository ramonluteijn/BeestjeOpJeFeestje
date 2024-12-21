using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Repository;

public class MainContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public MainContext(DbContextOptions<MainContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; } = null!;

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

        builder.Entity<Product>().HasData(
            new Product { Name = "Aap", Type = Type.JUNGLE, Price = 2000, Img = "monkey.png" },
            new Product { Name = "Olifant", Type = Type.JUNGLE, Price = 3000, Img = "elephant.png" },
            new Product { Name = "Zebra", Type = Type.JUNGLE, Price = 2500, Img = "zebra.png" },
            new Product { Name = "Leeuw", Type = Type.JUNGLE, Price = 3500, Img = "lion.png" },
            new Product { Name = "Hond", Type = Type.FARM, Price = 1000, Img = "dog.png" },
            new Product { Name = "Ezel", Type = Type.FARM, Price = 1500, Img = "donkey.png" },
            new Product { Name = "Koe", Type = Type.FARM, Price = 2000, Img = "cow.png" },
            new Product { Name = "Eend", Type = Type.FARM, Price = 500, Img = "rubber-duck.png" },
            new Product { Name = "Kuiken", Type = Type.FARM, Price = 250, Img = "chicken.png" },
            new Product { Name = "Pinguïn", Type = Type.SNOW, Price = 2000, Img = "penguin.png" },
            new Product { Name = "IJsbeer", Type = Type.SNOW, Price = 3000, Img = "polar-bear.png" },
            new Product { Name = "Zeehond", Type = Type.SNOW, Price = 2500, Img = "seal.png" },
            new Product { Name = "Kameel", Type = Type.DESERT, Price = 2000, Img = "camel.png" },
            new Product { Name = "Slang", Type = Type.DESERT, Price = 1500, Img = "snake.png" },
            new Product { Name = "T-Rex", Type = Type.VIP, Price = 5000, Img = "t-rex.png" },
            new Product { Name = "Unicorn", Type = Type.VIP, Price = 5000, Img = "unicorn.png" }
        );
    }
}