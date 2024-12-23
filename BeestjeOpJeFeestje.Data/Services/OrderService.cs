using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules;
using BeestjeOpJeFeestje.Repository;
using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestje.Data.Services;

public class OrderService(MainContext context, UserManager<User> userManager)
{
    public void CreateOrder(OrderDto orderDto, int? userId = null)
    {
        if (orderDto != null)
        {
            var discount = CheckRules(userId, orderDto);
            var order = new Order()
            {
                Name = orderDto.Name,
                Email = orderDto.Email,
                ZipCode = orderDto.ZipCode,
                HouseNumber = orderDto.HouseNumber,
                PhoneNumber = orderDto.PhoneNumber,
                OrderFor = orderDto.OrderFor,
                UserId = userId,
                TotalPrice = orderDto.TotalPrice * (100 - discount) / 100,
                OrderDetails = orderDto.OrderDetails.Select(x => new OrderDetail()
                {
                    ProductId = x.ProductId,
                }).ToList()
            };

            context.Orders.Add(order);
            context.SaveChanges();
        }
    }

    //convert to rules
    private int CheckRules(int? userId, OrderDto orderDto)
    {
        var user = userId != null ? userManager.FindByIdAsync(userId.ToString()).Result : null;

        var totalDiscount = new HasRankRule().UserHasRank(user)
                            + new CheckDayOfWeekRule().IsDayOfWeek(orderDto)
                            + new SameTypeRule().CheckSameType(ProductsInOrder(orderDto))
                            + new NameContainsRule().ApplyNameContainsDiscount(ProductsInOrder(orderDto))
                            + new CheckNameRule().CheckForName(ProductsInOrder(orderDto));

        return totalDiscount <= 60 ? totalDiscount : 60;
    }

    private List<ProductDto> ProductsInOrder(OrderDto orderDto)
    {
        return orderDto.OrderDetails.Select(x => new ProductDto()
        {
            Id = x.ProductId
        }).ToList();
    }

    public List<OrderDto> GetAllOrders()
    {
        return context.Orders
            .Select(x => new OrderDto()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                ZipCode = x.ZipCode,
                HouseNumber = x.HouseNumber,
                PhoneNumber = x.PhoneNumber,
                OrderFor = x.OrderFor,
                OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto()
                {
                    ProductId = y.ProductId
                }).ToList()
            })
            .ToList();
    }

    public OrderDto? GetOrder(int id)
    {
        return context.Orders
            .Where(x => x.Id == id)
            .Select(x => new OrderDto()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                ZipCode = x.ZipCode,
                HouseNumber = x.HouseNumber,
                PhoneNumber = x.PhoneNumber,
                OrderFor = x.OrderFor,
                OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto()
                {
                    ProductId = y.ProductId
                }).ToList()
            })
            .FirstOrDefault();
    }

    public void DeleteOrder(int id)
    {
        var order = context.Orders.FirstOrDefault(x => x.Id == id);

        if (order != null)
        {
            context.Orders.Remove(order);
            context.SaveChanges();
        }
    }

    public List<OrderDto> GetAllOrderByUserId(int id)
    {
        return context.Orders
            .Where(x => x.UserId == id)
            .Select(x => new OrderDto()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                ZipCode = x.ZipCode,
                HouseNumber = x.HouseNumber,
                PhoneNumber = x.PhoneNumber,
                OrderFor = x.OrderFor,
                OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto()
                {
                    ProductId = y.ProductId
                }).ToList()
            })
            .ToList();
    }
}