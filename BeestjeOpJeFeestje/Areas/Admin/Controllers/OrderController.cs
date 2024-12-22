using BeestjeOpJeFeestje.Areas.Admin.Models;
using BeestjeOpJeFeestje.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Areas.Admin.Controllers;

[Area("Admin")]
[Route("/admin/order")]
[Authorize]
public class OrderController(OrderService orderService): Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var orders = orderService.GetAllOrders();
        var ordersOverviewModel = new OrdersOverviewViewModel
        {
            Orders = orders
        };
        return View(ordersOverviewModel);
    }

    // [HttpGet("details/{id:int}")]
    // public IActionResult Details(int id)
    // {
    //     var order = orderService.GetOrderById(id);
    //     var orderViewModel = new SingleOrderViewModel()
    //     {
    //         Id = order.Id,
    //         Name = order.Name,
    //         Price = order.Price,
    //         Type = order.Type,
    //         Img = order.Img
    //     };
    //     return View(orderViewModel);
    // }
}