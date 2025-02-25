﻿using System.Security.Claims;
using BeestjeOpJeFeestje.Data.Services;
using BeestjeOpJeFeestje.Models.Orders;
using BeestjeOpJeFeestje.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Controllers;

[Route("/shop")]
public class OrderWizard(ProductService productService, BasketService basketService, OrderService orderService, AccountService accountService) : Controller
{
    [HttpGet]
    public IActionResult Index(string? message = "")
    {
        ViewBag.Message = message;
        basketService.ClearBasket();
        return View();
    }

    [HttpPost]
    public IActionResult IndexPost(DateOnly date)
    {
        if (date < DateOnly.FromDateTime(DateTime.Now))
        {
            ViewBag.Message = "You can't order for a date in the past";
            return View("Index");
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<Type>() });
    }

    [HttpGet("products")]
    public IActionResult Shop(DateOnly date, List<Type>? selectedTypes, string? result, bool check = true)
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
            Check = check,
            Result = result,
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
    public IActionResult Contact(DateOnly date, OrderViewModel? OVmodel = null)
    {
        var model = new OrderViewModel();

        if (OVmodel != null)
        {
            model.Name = OVmodel.Name;
            model.Email = OVmodel.Email;
            model.ZipCode = OVmodel.ZipCode;
            model.HouseNumber = OVmodel.HouseNumber;
            model.PhoneNumber = OVmodel.PhoneNumber;
            model.TotalPrice = OVmodel.TotalPrice;
            model.DiscountAmount = OVmodel.DiscountAmount;
        }
        model.OrderFor = date;
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
            SelectedTypes = new List<Type>(),
            BasketCount = basketService.GetBasketItemCount()
        };
        return View(model);
    }

    [HttpPost("contact")]
    public IActionResult ContactPost(OrderViewModel model, bool skip)
    {
        if (skip)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId != null)
            {
                var parsedId =  int.Parse(userId);
                var account = accountService.GetUserById(parsedId);
                model.Name = account.Name;
                model.Email = account.Email;
                model.ZipCode = account.ZipCode;
                model.OrderFor = model.OrderFor;
                model.HouseNumber = account.HouseNumber;
                model.PhoneNumber = account.PhoneNumber;

                ModelState.Clear();
                TryValidateModel(model);
            }
        }

        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };

        return ModelState.IsValid ? RedirectToAction("Confirmation", model) : RedirectToAction("Contact", new { date = model.OrderFor.ToString("yyyy-MM-dd"), OVmodel = model });
    }

   [HttpGet("confirmation")]
    public IActionResult Confirmation(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel     
        {
            Products = basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;
        model.DiscountAmount = model.TotalPrice * (100 - orderService.DiscountCheckRules(parsedId, model.ToDto())) / 100;

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

        orderService.CreateOrder(model.ToDto(), parsedId);
        basketService.ClearBasket();

        return RedirectToAction("Index", new { message = "Order created successfully, you may have payed for more products than expected :)" });
    }

    [HttpPost]
    [Route("AddToBasket")]
    public IActionResult AddToBasket(int productId, DateOnly date)
    {
        var product = productService.GetProductById(productId);
        if (product != null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parsedId = userId != null ? int.Parse(userId) : (int?)null;
            var (isValid, message) = basketService.AddToBasket(product, parsedId);
            if (!isValid)
            {
                return RedirectToAction("Shop", new { date, selectedTypes = new List<Type>(), result = message, check = isValid });
            }
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