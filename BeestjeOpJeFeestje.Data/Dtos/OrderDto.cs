namespace BeestjeOpJeFeestje.Data.Dtos;

public class OrderDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateOnly OrderFor { get; set; }
    public List<OrderDetailsDto> OrderDetails { get; set; } = new();
}