namespace BeestjeOpJeFeestje.Repository.Models;

public class Order
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
}