using System.Security.Claims;
using BeestjeOpJeFeestje.Data.Services;
using BeestjeOpJeFeestje.Models.Orders;
using BeestjeOpJeFeestje.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Controllers;

[Route("/shop")]
public class OrderWizard(ProductService productService, BasketService basketService, OrderService orderService, AccountService accountService) : Controller
{
    [HttpGet]
    public IActionResult Index(string? message = "")
    {
        ViewBag.Message = message;
        basketService.ClearBasket();
        return View();
    }

    [HttpPost]
    public IActionResult IndexPost(DateOnly date)
    {
        if (date < DateOnly.FromDateTime(DateTime.Now))
        {
            ViewBag.Message = "You can't order for a date in the past";
            return View("Index");
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<Type>() });
    }

    [HttpGet("products")]
    public IActionResult Shop(DateOnly date, List<Type>? selectedTypes)
    {
        var products = productService.GetProducts(date, selectedTypes);
        var basketProducts = basketService.GetBasketProducts();

        foreach (var product in products)
        {
            product.IsInBasket = basketProducts.Any(bp => bp.Id == product.Id);
        }

        var model = new OrderViewModel()
        {
            OrderFor = date,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = products,
                SelectedTypes = selectedTypes ?? new List<Type>(),
                BasketCount = basketService.GetBasketItemCount()
            }
        };
        return View(model);
    }

    [HttpGet("contact")]
    public IActionResult Contact(DateOnly date)
    {
        var model = new OrderViewModel()
        {
            OrderFor = date,
            ProductsOverViewModel = new ProductsOverViewModel
            {
                Products = basketService.GetBasketProducts(),
                SelectedTypes = new List<Type>(),
                BasketCount = basketService.GetBasketItemCount()
            },
        };
        return View(model);
    }

    //todo
    [HttpPost("contact")]
    public IActionResult ContactPost(OrderViewModel model, bool skip)
    {
        if (skip)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId != null)
            {
                var parsedId =  int.Parse(userId);
                var account = accountService.GetUserById(parsedId);
                model.Name = account.Name;
                model.Email = account.Email;
                model.ZipCode = account.ZipCode;
                model.HouseNumber = account.HouseNumber;
                model.PhoneNumber = account.PhoneNumber;

                ModelState.Clear();
                TryValidateModel(model);
            }
        }

        //todo products not populating correctly
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };

        return ModelState.IsValid ? RedirectToAction("Confirmation", model) : RedirectToAction("Contact");
    }

   [HttpGet("confirmation")]
    public IActionResult Confirmation(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel     
        {
            Products = basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;
        model.DiscountAmount = model.TotalPrice * (100 - orderService.DiscountCheckRules(parsedId, model.ToDto())) / 100;

        return View(model);
    }

    [HttpPost("confirmation")]
    public IActionResult ConfirmationPost(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };
        model.TotalPrice = model.ProductsOverViewModel.Products.Sum(p => p.Price);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        var parsedId = userId != null ? int.Parse(userId) : (int?)null;

        var (check, result) = orderService.CreateOrder(model.ToDto(), parsedId);
        if (check)
        {
            basketService.ClearBasket();
        }
        model.Check = check;
        model.Result = result;
        return RedirectToAction("Confirmation", model); //redirect to different page with order confirmation todo
    }

    [HttpPost]
    [Route("AddToBasket")]
    public IActionResult AddToBasket(int productId, DateOnly date)
    {
        var product = productService.GetProductById(productId);
        if (product != null)
        {
            basketService.AddToBasket(product);
        }
        return RedirectToAction("Shop", new { date, selectedTypes = new List<Type>() });
    }

    [HttpPost]
    [Route("RemoveFromBasket")]
    public IActionResult RemoveFromBasket(int productId, DateOnly date)
    {
         basketService.RemoveFromBasket(productId);
         return RedirectToAction("Shop", new { date, selectedTypes = new List<Type>() });
    }
}