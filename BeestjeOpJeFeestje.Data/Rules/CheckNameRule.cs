using BeestjeOpJeFeestje.Data.Dtos;
using System;

namespace BeestjeOpJeFeestje.Data.Rules;

public class CheckNameRule
{
    private static readonly Random random = new Random();
    private static readonly string name = "eend";

    //50% discount if the order contains a product with the name "eend"
    public int CheckForName(OrderDto orderDto)
    {
        if (orderDto == null)
        {
            throw new ArgumentNullException(nameof(orderDto));
        }
        if(orderDto.OrderDetails.Any(p => p.Product.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            return random.Next(1, 7) == 1 ? 50 : 0;
        }
        return 0;
    }
}