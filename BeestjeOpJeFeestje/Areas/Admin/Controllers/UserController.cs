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

    [HttpGet("details{id}")]
    public IActionResult Details(int id)
    {
        var user = accountService.GetUserById(id);
        var userViewModel = new SingleUserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            HouseNumber = user.HouseNumber,
            ZipCode = user.ZipCode,
            PhoneNumber = user.PhoneNumber,
            Rank = user.Rank
        };
        return View(userViewModel);
    }

    [HttpGet("edit{id}")]
    public IActionResult Edit(int id)
    {
        var user = accountService.GetUserById(id);
        var userViewModel = new SingleUserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            HouseNumber = user.HouseNumber,
            ZipCode = user.ZipCode,
            PhoneNumber = user.PhoneNumber,
            Rank = user.Rank
        };
        return View(userViewModel);
    }

    [HttpPost("edit{id}")]
    public async Task<IActionResult> Edit(SingleUserViewModel userViewModel)
    {
        if (ModelState.IsValid)
        {
            var userDto = userViewModel.ToDto();
            try
            {
                await accountService.UpdateUser(userDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        return View(userViewModel);
    }
}