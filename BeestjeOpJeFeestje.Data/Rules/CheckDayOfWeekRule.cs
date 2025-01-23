using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Rules;

public class CheckDayOfWeekRule
{
    //15% discount on Monday and Tuesday
    public int IsDayOfWeek(OrderDto orderDto)
    {
        if (orderDto == null)
        {
            throw new ArgumentNullException(nameof(orderDto));
        }
        return orderDto.OrderFor.DayOfWeek == DayOfWeek.Monday || orderDto.OrderFor.DayOfWeek == DayOfWeek.Tuesday ? 15 : 0;
    }
}


