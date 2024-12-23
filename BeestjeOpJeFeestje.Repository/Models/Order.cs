namespace BeestjeOpJeFeestje.Repository.Models;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public DateOnly OrderFor { get; set; }
    public int? UserId { get; set; }
    public int TotalPrice { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = new();
}