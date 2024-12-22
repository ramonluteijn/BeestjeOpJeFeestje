using BeestjeOpJeFeestje.Areas.Customer.Models;
using BeestjeOpJeFeestje.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;
namespace BeestjeOpJeFeestje.Areas.Customer.Controllers;

[Area("Customer")]
[Route("/shop")]
public class OrderWizard(ProductService productService, BasketService basketService, OrderService orderService, ILogger<OrderWizard> logger) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("products")]
    public IActionResult Shop(DateOnly date, List<Type>? selectedTypes)
    {
        var products = productService.GetProducts();
        var basketProducts = basketService.GetBasketProducts();

        foreach (var product in products)
        {
            product.IsInBasket = basketProducts.Any(bp => bp.Id == product.Id);
        }

        // todo refactor to get products based on date and type
        if (selectedTypes != null && selectedTypes.Any())
        {
            products = products.Where(p => selectedTypes.Contains(p.Type)).ToList();
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
    public IActionResult ContactPost(OrderViewModel model)
    {
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
        return View(model);
    }

    [HttpPost("confirmation")]
    public IActionResult ConfirmationPost(OrderViewModel model)
    {
        model.ProductsOverViewModel = new ProductsOverViewModel
        {
            Products = basketService.GetBasketProducts(),
        };

        orderService.CreateOrder(model.ToDto());
        basketService.ClearBasket();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("AddToBasket")]
    public IActionResult AddToBasket(int productId)
    {
        var product = productService.GetProductById(productId);
        if (product != null)
        {
            basketService.AddToBasket(product);
        }
        return RedirectToAction("Shop", new { date = DateTime.Now.Date, selectedTypes = new List<Type>() });
    }

    [HttpPost]
    [Route("RemoveFromBasket")]
    public IActionResult RemoveFromBasket(int productId)
    {
         basketService.RemoveFromBasket(productId);
         return RedirectToAction("Shop", new { date = DateTime.Now.Date, selectedTypes = new List<Type>() });
    }
}