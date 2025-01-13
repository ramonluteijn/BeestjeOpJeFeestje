using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Models.Orders;

public class OrdersOverviewViewModel
{
    public IEnumerable<OrderDto> Orders { get; set; }
}