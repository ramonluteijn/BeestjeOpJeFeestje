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

    [HttpGet("{id:int}/details")]

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

    [HttpGet("{id:int}/edit")]

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

    [HttpPost("{id:int}/edit")]

    public async Task<IActionResult> Edit(int id, SingleUserViewModel userViewModel)
    {
        if (ModelState.IsValid)
        {
            var userDto = userViewModel.ToDto();
            try
            {
                var(check, result) = await accountService.UpdateUser(id, userDto);
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

    [HttpGet("delete{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await accountService.DeleteUser(id);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
        return RedirectToAction("Index");
    }
}