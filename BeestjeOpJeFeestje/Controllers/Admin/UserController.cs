using BeestjeOpJeFeestje.Data.Services;
using BeestjeOpJeFeestje.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("/admin/user")]
public class UserController(AccountService accountService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var customers = accountService.GetAllUsers();
        var usersOverviewModel = new UsersOverviewViewModel { customers = customers };
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
        await HandleUserSave(userViewModel, true);
        return View(userViewModel);
    }

    [HttpGet("{id:int}/details")]
    public IActionResult Details(int id)
    {
        var userViewModel = GetUserViewModel(id);
        return View(userViewModel);
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        var userViewModel = GetUserViewModel(id);
        return View(userViewModel);
    }

    [HttpPost("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, SingleUserViewModel userViewModel)
    {
        await HandleUserSave(userViewModel, false, id);
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

    private async Task HandleUserSave(SingleUserViewModel userViewModel, bool isCreate, int? id = null)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var userDto = userViewModel.ToDto();
                var (check, result) = isCreate ? await accountService.CreateUser(userDto) : await accountService.UpdateUser(id.Value, userDto);
                userViewModel.Check = check;
                userViewModel.Result = result;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
            }
        }
    }

    private SingleUserViewModel GetUserViewModel(int id)
    {
        var user = accountService.GetUserById(id);
        return new SingleUserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            HouseNumber = user.HouseNumber,
            ZipCode = user.ZipCode,
            PhoneNumber = user.PhoneNumber,
            Rank = user.Rank
        };
    }
}