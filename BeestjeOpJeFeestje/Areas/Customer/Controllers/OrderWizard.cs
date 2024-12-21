using BeestjeOpJeFeestje.Areas.Customer.Models;
using BeestjeOpJeFeestje.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;
namespace BeestjeOpJeFeestje.Areas.Customer.Controllers;

[Area("Customer")]
[Route("/shop")]
public class OrderWizard(ProductService productService, BasketService basketService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("products")]
    public IActionResult Shop(DateOnly date, List<Type> selectedTypes)
    {
        var products = productService.GetProducts();
        var basketProducts = basketService.GetBasketProducts();

        foreach (var product in products)
        {
            product.IsInBasket = basketProducts.Any(bp => bp.Id == product.Id);
        }

        if (selectedTypes != null && selectedTypes.Any())
        {
            products = products.Where(p => selectedTypes.Contains(p.Type)).ToList();
        }

        var model = new ProductsOverViewModel
        {
            Products = products,
            Date = date,
            SelectedTypes = selectedTypes ?? new List<Type>(),
            BasketCount = basketService.GetBasketItemCount()
        };
        return View(model);
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