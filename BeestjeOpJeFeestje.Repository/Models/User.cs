using BeestjeOpJeFeestje.Repository.Enums;
using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestje.Repository.Models;

public class User : IdentityUser<int>
{
    public Rank Rank { get; set; }
    public string HouseNumber { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
}