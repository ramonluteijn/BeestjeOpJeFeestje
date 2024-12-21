using BeestjeOpJeFeestje.Areas.Customer.Models;
using BeestjeOpJeFeestje.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;
namespace BeestjeOpJeFeestje.Areas.Customer.Controllers;

[Area("Customer")]
[Route("/shop")]
public class ShopController(ProductService productService, BasketService basketService) : Controller
{
    [HttpGet]
    public IActionResult Index(DateOnly date, List<Type> selectedTypes)
    {
        var products = productService.GetProducts();
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
        return RedirectToAction("Index", new { date = DateTime.Now.Date, selectedTypes = new List<Type>() });
    }

    [HttpGet]
    [Route("ViewBasket")]
    public IActionResult ViewBasket()
    {
        var products = basketService.GetBasketProducts();
        return View(products);
    }

    [HttpPost]
    [Route("RemoveFromBasket")]
    public IActionResult RemoveFromBasket(int productId)
    {
         basketService.RemoveFromBasket(productId);
         return RedirectToAction("ViewBasket");
    }
}