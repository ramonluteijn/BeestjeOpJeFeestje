using System.Globalization;
using BeestjeOpJeFeestje.Data.Services;

using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Areas.Auth.Controllers;

[Area("Auth")]
public class AccountController(AccountService accountService) : Controller
{
    [Route("/login")]
    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    [Route("/")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginCheck(string loginInput, string password)
    {
        var user = await accountService.Login(loginInput, password);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (User.IsInRole("Customer"))
        {
            return RedirectToAction("Index", "", new {area = "Customer"}); // TODO: Add controller name
        }
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("Index", "User", new { area = "Admin" }); //TODO: Add controller name
        }

        throw new InvalidOperationException("Invalid account role");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await accountService.Logout();
        return RedirectToAction("Login", "Account");
    }
}