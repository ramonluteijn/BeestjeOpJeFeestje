using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class NameContainsRule
{
    //2% discount for each unique letter in the product name
    public int ApplyNameContainsDiscount(OrderDto orderDto)
    {

        var uniqueLetters = new HashSet<char>();

        foreach (var product in orderDto.OrderDetails.Select(p => p.Product))
        {
            foreach (var letter in product.Name.ToLower())
            {
                if (!uniqueLetters.Contains(letter))
                {
                    uniqueLetters.Add(letter);
                }
            }
        }
        return uniqueLetters.Count * 2;
    }
}