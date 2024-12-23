using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class SameTypeRule
{
    private readonly int sameAmount = 3;
    public int CheckSameType(List<ProductDto> products)
    {
        return products.GroupBy(p => p.Type).Any(g => g.Count() >= sameAmount) ? 10 : 0;
    }
}