using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Data.Rules;

public class CheckOrderProductsRule
{
    private readonly int NONE_MAX = 3;
    private readonly int SILVER_MAX = 4;

    public bool CheckProducts(OrderDto orderDto, User user)
    {
        var amountOfProducts = orderDto.OrderDetails.Count;
        var hasVipProduct = orderDto.OrderDetails.Any(p => p.Product.Type == Type.VIP);

        switch (user.Rank)
        {
            case Rank.PLATINUM:
                return true;
            case Rank.GOLD:
                return !hasVipProduct;
            case Rank.SILVER:
                return amountOfProducts <= SILVER_MAX && !hasVipProduct;
            case Rank.NONE:
            default:
                return amountOfProducts <= NONE_MAX && !hasVipProduct;
        }
    }
}