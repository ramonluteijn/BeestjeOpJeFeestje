using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class PayForMoreProducts
{
    private static readonly int requiredAmount = 2;
    private static readonly int payForAmount = 3;
    private static readonly Random random = new Random();

    //Check if the customer needs to pay for extra products
    public int PayForExtraProducts(OrderDto orderDto, List<ProductDto> allProducts)
    {
        if (orderDto == null)
        {
            throw new ArgumentNullException(nameof(orderDto));
        }

        if (orderDto.OrderDetails == null || orderDto.OrderDetails.Count == 0)
        {
            return 0;
        }

        if (orderDto.OrderDetails.Count % requiredAmount == 0)
        {
            var randomProduct = allProducts[random.Next(allProducts.Count)];
            return randomProduct.Price;
        }
        return 0;
    }
}