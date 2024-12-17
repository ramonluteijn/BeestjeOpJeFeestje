using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
[Route("/admin/user")]
public class UserController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}