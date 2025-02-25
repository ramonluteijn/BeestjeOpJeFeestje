﻿using System.ComponentModel.DataAnnotations;
using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules.ValidationRules;
using BeestjeOpJeFeestje.Models.Products;

namespace BeestjeOpJeFeestje.Models.Orders;

public class OrderViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [PostalCodeRule] // werkt niet
    public string ZipCode { get; set; } = null!;

    [Required]
    [StringLength(5)]
    public string HouseNumber { get; set; } = null!;

    [Required]
    [DataType(DataType.PhoneNumber)] // werkt niet
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly OrderFor { get; set; }
    public ProductsOverViewModel ProductsOverViewModel { get; set; } = new();
    public int TotalPrice { get; set; }
    public int DiscountAmount { get; set; }

    public bool Check{ get; set; }
    public string? Result { get; set; }

    public OrderDto ToDto()
    {
        return new OrderDto()
        {
            Name = this.Name,
            Email = this.Email,
            HouseNumber = this.HouseNumber,
            ZipCode = this.ZipCode,
            PhoneNumber = this.PhoneNumber,
            OrderFor = this.OrderFor,
            TotalPrice = this.TotalPrice,
            OrderDetails = this.ProductsOverViewModel.Products
                .Where(p => p.IsInBasket)
                .Select(p => new OrderDetailsDto()
                {
                    ProductId = p.Id,
                    Product = p
                })
                .ToList()
        };
    }
}