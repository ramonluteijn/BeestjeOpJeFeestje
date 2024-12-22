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
}