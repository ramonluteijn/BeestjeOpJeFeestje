using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class NameContainsRule
{
    public int ApplyNameContainsDiscount(List<ProductDto> products)
    {
        var uniqueLetters = new HashSet<char>();

        foreach (var product in products)
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