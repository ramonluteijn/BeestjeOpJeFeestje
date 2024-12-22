using System.Security.Claims;
using BeestjeOpJeFeestje.Areas.Customer.Models;
using BeestjeOpJeFeestje.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Areas.Customer.Controllers;

[Area("Customer")]
[Route("/customer/order")]
[Authorize]
public class OrderController(OrderService orderService): Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var orders = orderService.GetAllOrderByUserId(int.Parse(userId));
        var ordersOverviewModel = new OrdersOverviewViewModel
        {
            Orders = orders
        };
        return View(ordersOverviewModel);
    }

    [HttpGet("details/{id:int}")]
    public IActionResult Details(int id)
    {
        var order = orderService.GetOrder(id);
        var model = new OrderViewModel
        {
            Id = order!.Id,
            Name = order.Name,
            Email = order.Email,
            ZipCode = order.ZipCode,
            HouseNumber = order.HouseNumber,
            PhoneNumber = order.PhoneNumber,
            OrderFor = order.OrderFor
            //also return product data

        };
        return View(model);
    }

    // [HttpGet("edit/{id:int}")]
    // public IActionResult Edit(int id)
    // {
    //     var order = orderService.GetOrderById(id);
    //     var orderViewModel = new OrderViewModel
    //     {
    //         Id = order.Id,
    //         Name = order.Name,
    //         Email = order.Email,
    //         ZipCode = order.ZipCode,
    //         HouseNumber = order.HouseNumber,
    //         PhoneNumber = order.PhoneNumber,
    //         OrderFor = order.OrderFor
    //     };
    //     return View(orderViewModel);
    // }
    //
    // [HttpPost("edit")]
    // public IActionResult Edit(OrderViewModel orderViewModel)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         orderService.UpdateOrder(orderViewModel.ToDto());
    //         return RedirectToAction("Index");
    //     }
    //     return View(orderViewModel);
    // }
    //
    // [HttpGet("delete/{id:int}")]
    // public IActionResult Delete(int id)
    // {
    //     orderService.DeleteOrder(id);
    //     return RedirectToAction("Index");
    // }
}