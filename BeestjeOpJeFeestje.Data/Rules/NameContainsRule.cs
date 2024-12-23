using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class NameContainsRule
{
    public int ApplyNameContainsDiscount(OrderDto orderDto)
    {

        var uniqueLetters = new HashSet<char>();

        foreach (var product in orderDto.OrderDetails.Select(p => p.Product))
        {
            foreach (var letter in product.Name.ToLower())
            {
                if (char.IsLetter(letter))
                {
                    uniqueLetters.Add(letter);
                }
            }
        }
        return uniqueLetters.Count * 2;
    }
}