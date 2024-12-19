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

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(SingleUserViewModel userViewModel)
    {
        if (ModelState.IsValid)
        {
            var userDto = userViewModel.ToDto();
            try
            {
                var (check, result) = await accountService.CreateUser(userDto);
                userViewModel.Check = check;
                userViewModel.Result = result;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        return View(userViewModel);
    }
}