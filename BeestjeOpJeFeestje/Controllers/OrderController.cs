using System.Security.Claims;
using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Services;
using BeestjeOpJeFeestje.Models.Orders;
using BeestjeOpJeFeestje.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers;

[Route("/order")]
[Authorize]
public class OrderController(OrderService orderService): Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var orders = User.IsInRole("Admin") ? orderService.GetAllOrders() : orderService.GetAllOrderByUserId(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        return View(new OrdersOverviewViewModel { Orders = orders });
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
            OrderFor = order.OrderFor,
            TotalPrice = order.TotalPrice,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = order.OrderDetails.Select(od => od.Product).ToList()
            }
        };
        return View(model);
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        orderService.DeleteOrder(id);
        return RedirectToAction("Index");
    }
}