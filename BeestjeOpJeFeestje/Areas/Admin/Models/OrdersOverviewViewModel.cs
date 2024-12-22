using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Areas.Admin.Models;

public class OrdersOverviewViewModel
{
    public IEnumerable<OrderDto> Orders { get; set; }
}