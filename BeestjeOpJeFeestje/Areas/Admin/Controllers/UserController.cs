using BeestjeOpJeFeestje.Areas.Admin.Models;
using BeestjeOpJeFeestje.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
[Route("/admin/user")]
public class UserController(AccountService accountService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var customers = accountService.GetAllUsers();
        var usersOverviewModel = new UsersOverviewViewModel
        {
            customers = customers
        };

        return View(usersOverviewModel);
    }
}