using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Areas.Customer.Models;

public class OrdersOverviewViewModel
{
    public IEnumerable<OrderDto> Orders { get; set; }
}