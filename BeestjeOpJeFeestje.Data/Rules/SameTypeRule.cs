using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class SameTypeRule
{
    private readonly int sameAmount = 3;

    //10% discount if the order contains 3 or more products of the same type
    public int CheckSameType(OrderDto orderDto)
    {
        return orderDto.OrderDetails.GroupBy(p => p.Product.Type).Any(g => g.Count() >= sameAmount) ? 10 : 0;
    }
}