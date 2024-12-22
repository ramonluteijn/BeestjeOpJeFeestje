using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Repository;
using BeestjeOpJeFeestje.Repository.Models;

namespace BeestjeOpJeFeestje.Data.Services;

public class OrderService(MainContext context)
{
    public void CreateOrder(OrderDto orderDto, int? userId = null)
    {
        if (orderDto != null)
        {
            var order = new Order()
            {
                Name = orderDto.Name,
                Email = orderDto.Email,
                ZipCode = orderDto.ZipCode,
                HouseNumber = orderDto.HouseNumber,
                PhoneNumber = orderDto.PhoneNumber,
                OrderFor = orderDto.OrderFor,
                UserId = userId,
                OrderDetails = orderDto.OrderDetails.Select(x => new OrderDetail()
                {
                    ProductId = x.ProductId,
                }).ToList()
            };

            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}