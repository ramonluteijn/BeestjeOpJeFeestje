using BeestjeOpJeFeestje.Data.Dtos;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Data.Rules;

public class ProductsMayNotBeTogether
{
    //Check if the animal is available in the current season
    public (bool, string) CheckProductsTogether(OrderDto orderDto)
    {
        var dangerousAnimals = new[] { "leeuw", "ijsbeer" }; //  names of animals not types!

        foreach (var orderDetail in orderDto.OrderDetails)
        {
            if (dangerousAnimals.Contains(orderDetail.Product.Name.ToLower()))
            {
                foreach (var orderDetail2 in orderDto.OrderDetails)
                {
                    if (orderDetail2.Product.Type == Type.FARM)
                    {
                        return (false, "Nom Nom Nom");
                    }
                }
            }
        }
        return (true, string.Empty);
    }
}