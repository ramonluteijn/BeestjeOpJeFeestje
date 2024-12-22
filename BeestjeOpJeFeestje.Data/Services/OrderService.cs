﻿using BeestjeOpJeFeestje.Data.Dtos;
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