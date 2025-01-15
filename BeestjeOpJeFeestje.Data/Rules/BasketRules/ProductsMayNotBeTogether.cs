using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Models;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Data.Rules.BasketRules;

public class ProductsMayNotBeTogether
{
    // Check if the animal is available in the current season
    public (bool, string) CheckProductsTogether(Basket basket, ProductDto product)
    {
        var dangerousAnimals = new[] { "leeuw", "ijsbeer" }; // names of animals not types!

        if (dangerousAnimals.Contains(product.Name.ToLower()))
        {
            foreach (var product2 in basket.Products)
            {
                if (product2.Type == Type.FARM)
                {
                    return (false, "Nom Nom Nom");
                }
            }
        }
        else if (product.Type == Type.FARM)
        {
            foreach (var product2 in basket.Products)
            {
                if (dangerousAnimals.Contains(product2.Name.ToLower()))
                {
                    return (false, "Nom Nom Nom");
                }
            }
        }
        return (true, string.Empty);
    }
}