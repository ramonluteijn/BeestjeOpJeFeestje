using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules;
using BeestjeOpJeFeestje.Repository;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestje.Data.Services;

public class OrderService(MainContext context, UserManager<User> userManager, ProductService productService)
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
                TotalPrice = CalculateTotalPrice(orderDto, userId),
                OrderDetails = orderDto.OrderDetails.Select(x => new OrderDetail()
                {
                    ProductId = x.ProductId,
                    Product = context.Products.FirstOrDefault(p => p.Id == x.ProductId)
                }).ToList()
            };

            context.Orders.Add(order);
            context.SaveChanges();
        }
    }

    private int CalculateTotalPrice(OrderDto orderDto, int? userId)
    {
        var discount = DiscountCheckRules(userId, orderDto);
        var extraCost = new PayForMoreProducts().PayForExtraProducts(orderDto, productService.GetProducts());
        return orderDto.TotalPrice * (100 - discount) / 100 + extraCost;
    }

    public int DiscountCheckRules(int? userId, OrderDto orderDto)
    {
        var user = userId != null ? userManager.FindByIdAsync(userId.ToString()).Result : null;

        var totalDiscount = new HasRankRule().UserHasRank(user)
                            + new CheckDayOfWeekRule().IsDayOfWeek(orderDto)
                            + new SameTypeRule().CheckSameType(orderDto)
                            + new NameContainsRule().ApplyNameContainsDiscount(orderDto)
                            + new CheckNameRule().CheckForName(orderDto);

        return totalDiscount <= 60 ? totalDiscount : 60;
    }

    private IQueryable<OrderDto> SelectAllOrders()
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
                TotalPrice = x.TotalPrice,
                OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto()
                {
                    ProductId = y.ProductId,
                    Product = new ProductDto()
                    {
                        Id = y.Product.Id,
                        Name = y.Product.Name,
                        Type = y.Product.Type,
                        Price = y.Product.Price,
                        Img = y.Product.Img
                    }
                }).ToList()
            });
    }
    public List<OrderDto> GetAllOrders()
    {
        return SelectAllOrders().ToList();
    }

    public OrderDto? GetOrder(int id)
    {
        return SelectAllOrders().FirstOrDefault(x => x.Id == id);
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
                TotalPrice = x.TotalPrice,
                OrderDetails = x.OrderDetails.Select(y => new OrderDetailsDto()
                {
                    ProductId = y.ProductId
                }).ToList()
            })
            .ToList();
    }

    public List<OrderDto> GetAllOrdersByProductId(int id)
    {
        return SelectAllOrders()
            .Where(x => x.OrderDetails.Any(y => y.ProductId == id))
            .ToList();
    }
}