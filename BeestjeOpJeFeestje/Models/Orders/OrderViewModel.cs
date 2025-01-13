using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Models.Products;

namespace BeestjeOpJeFeestje.Models.Orders;

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

    public string? Result { get; set; }
    public bool Check { get; set; }

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
                    ProductId = p.Id,
                    Product = p
                })
                .ToList()
        };
    }
}