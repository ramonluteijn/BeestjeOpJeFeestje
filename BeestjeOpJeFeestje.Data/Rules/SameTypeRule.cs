using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class SameTypeRule
{
    private readonly int sameAmount = 3;
    public int CheckSameType(OrderDto orderDto)
    {
        return orderDto.OrderDetails.GroupBy(p => p.Product.Type).Any(g => g.Count() >= sameAmount) ? 10 : 0;
    }
}