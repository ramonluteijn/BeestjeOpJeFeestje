using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Areas.Customer.Models;

public class OrderViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateOnly OrderFor { get; set; }
    public ProductsOverViewModel ProductsOverViewModel { get; set; } = new();
    public int TotalPrice { get; set; }

    public OrderDto ToDto()
    {
        return new OrderDto()
        {
            Name = this.Name,
            Email = this.Email,
            HouseNumber = this.HouseNumber,
            ZipCode = this.ZipCode,
            PhoneNumber = this.PhoneNumber,
            OrderFor = this.OrderFor,
            TotalPrice = this.TotalPrice,
            OrderDetails = this.ProductsOverViewModel.Products
                .Where(p => p.IsInBasket)
                .Select(p => new OrderDetailsDto()
                {
                    ProductId = p.Id
                })
                .ToList()
        };
    }
}