using BeestjeOpJeFeestje.Data.Services;
using BeestjeOpJeFeestje.Repository;
using BeestjeOpJeFeestje.Repository.Factories;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        // Password settings
        options.Password.RequireDigit = false; // Allow no digits
        options.Password.RequireLowercase = false; // Allow no lowercase letters
        options.Password.RequireUppercase = false; // Allow no uppercase letters
        options.Password.RequireNonAlphanumeric = false; // Allow no special characters
        options.Password.RequiredLength = 1; // Minimum length of 1 character
        options.Password.RequiredUniqueChars = 1; // At least 1 unique character
    })
    .AddEntityFrameworkStores<MainContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddSingleton<BasketService>();
builder.Services.AddScoped<ImageFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
