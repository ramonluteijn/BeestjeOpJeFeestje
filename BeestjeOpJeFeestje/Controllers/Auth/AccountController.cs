using BeestjeOpJeFeestje.Data.Services;

using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers.Auth;

[Route("/auth")]
public class AccountController(AccountService accountService) : Controller
{
    [Route("/auth")]
    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    [Route("/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("/login/check")]
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
            return RedirectToAction("Index", "User");
        }

        throw new InvalidOperationException("Invalid account role");
    }

    [HttpPost]
    [Route("/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await accountService.Logout();
        return RedirectToAction("Login", "Account");
    }
}