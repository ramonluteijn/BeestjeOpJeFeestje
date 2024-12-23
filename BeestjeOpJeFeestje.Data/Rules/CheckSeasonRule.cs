using BeestjeOpJeFeestje.Data.Dtos;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Data.Rules;

public class CheckSeasonRule
{
    public (bool, string) CheckAnimalAvailability(OrderDto orderDto)
    {
        var currentDate = DateTime.Now;
        var dayOfWeek = currentDate.DayOfWeek;
        var month = currentDate.Month;

        foreach (var orderDetail in orderDto.OrderDetails)
        {
            var productType = orderDetail.Product.Type;

            if (orderDetail.Product.Name.Equals("Pinguïn") && (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday))
            {
                return (false, "Dieren in pak werken alleen doordeweek");
            }

            if (productType == Type.DESERT && (month >= 10 || month <= 2))
            {
                return (false, "Veelste koud");
            }

            if (productType == Type.SNOW && (month >= 6 && month <= 8))
            {
                return (false, "Some People Are Worth Melting For. ~ Ola");
            }
        }

        return (true, string.Empty);
    }
}