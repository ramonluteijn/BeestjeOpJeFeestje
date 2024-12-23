using BeestjeOpJeFeestje.Data.Dtos;
using System;

namespace BeestjeOpJeFeestje.Data.Rules;

public class CheckNameRule
{
    private static readonly Random random = new Random();
    private static readonly string name = "eend";

    public int CheckForName(List<ProductDto> products)
    {
        if (products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            return random.Next(1, 7) == 1 ? 50 : 0;
        }
        return 0;
    }
}