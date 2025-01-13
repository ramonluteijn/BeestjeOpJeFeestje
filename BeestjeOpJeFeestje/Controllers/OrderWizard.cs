﻿using System.Security.Claims;
using BeestjeOpJeFeestje.Data.Services;
using BeestjeOpJeFeestje.Models.Orders;
using BeestjeOpJeFeestje.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Controllers;

[Route("/shop")]
public class OrderWizard(ProductService productService, BasketService basketService, OrderService orderService, ILogger<OrderWizard> logger) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("products")]
    public IActionResult Shop(DateOnly date, List<Type>? selectedTypes)
    {
        var products = productService.GetProducts(date, selectedTypes);
        var basketProducts = basketService.GetBasketProducts();

        foreach (var product in products)
        {
            product.IsInBasket = basketProducts.Any(bp => bp.Id == product.Id);
        }

        var model = new OrderViewModel()
        {
            OrderFor = date,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = products,
                SelectedTypes = selectedTypes ?? new List<Type>(),
                BasketCount = basketService.GetBasketItemCount()
            }
        };
        return View(model);
    }

    [HttpGet("contact")]
    public IActionResult Contact(DateOnly date)
    {
        var model = new OrderViewModel()
        {
            OrderFor = date,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = basketService.GetBasketProducts(),
                SelectedTypes = new List<Type>(),
                BasketCount = basketService.GetBasketItemCount()
            },
        };
        return View(model);
    }

    //todo
    [HttpPost("contact")]
    public IActionResult ContactPost(OrderViewModel model)
    {
//todo products not populating correctly
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };

        return ModelState.IsValid ? RedirectToAction("Confirmation", model) : RedirectToAction("Contact");
    }

   [HttpGet("confirmation")]
    public IActionResult Confirmation(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel     
        {
            Products = basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        return View(model);
    }

    [HttpPost("confirmation")]
    public IActionResult ConfirmationPost(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;

        var (check, result) = orderService.CreateOrder(model.ToDto(), parsedId);
        if (check)
        {
            basketService.ClearBasket();
        }
        model.Check = check;
        model.Result = result;
        return RedirectToAction("Confirmation", model); //redirect to different page with order confirmation todo
    }

    [HttpPost]
    [Route("AddToBasket")]
    public IActionResult AddToBasket(int productId, DateOnly date)
    {
        var product = productService.GetProductById(productId);
        if (product != null)
        {
            basketService.AddToBasket(product);
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<Type>() });
    }

    [HttpPost]
    [Route("RemoveFromBasket")]
    public IActionResult RemoveFromBasket(int productId, DateOnly date)
    {
         basketService.RemoveFromBasket(productId);
         return RedirectToAction("Shop", new { date, selectedTypes = new List<Type>() });
    }
}