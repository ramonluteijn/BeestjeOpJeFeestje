using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Data.Rules;

public class CheckOrderProductsRule
{
    private readonly int NONE_MAX = 3;
    private readonly int SILVER_MAX = 4;

    //Check if the order is valid
    public (bool, string) CheckProducts(OrderDto orderDto, User? user)
    {
        var amountOfProducts = orderDto.OrderDetails.Count;
        var hasVipProduct = orderDto.OrderDetails.Any(p => p.Product.Type == Type.VIP);

        if(user == null)
        {
            return amountOfProducts <= NONE_MAX && !hasVipProduct ? (true, string.Empty) : (false, "You have too many products in your order, only" + NONE_MAX + " products are allowed and no VIP products.");
        }
        return user.Rank switch
        {
            Rank.PLATINUM => (true, string.Empty),
            Rank.GOLD => !hasVipProduct ? (true, string.Empty) : (false, "Gold members cannot have VIP products."),
            Rank.SILVER => amountOfProducts <= SILVER_MAX && !hasVipProduct ? (true, string.Empty) : (false, "Silver members can only have up to " + SILVER_MAX + " products and no VIP products."),
            Rank.NONE => amountOfProducts <= NONE_MAX && !hasVipProduct ? (true, string.Empty) : (false, "You have too many products in your order, only " + NONE_MAX + " products are allowed and no VIP products."),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}