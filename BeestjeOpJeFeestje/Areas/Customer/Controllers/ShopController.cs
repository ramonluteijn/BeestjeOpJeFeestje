using BeestjeOpJeFeestje.Areas.Customer.Models;
using BeestjeOpJeFeestje.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("/shop")]
    public class ShopController(ProductService productService) : Controller
    {
        [HttpGet]
        public IActionResult Index(List<Type> selectedTypes)
        {
            var products = productService.GetProducts();

            if (selectedTypes != null && selectedTypes.Any())
            {
                products = products.Where(p => selectedTypes.Contains(p.Type)).ToList();
            }

            var model = new ProductsOverViewModel
            {
                Products = products,
                SelectedTypes = selectedTypes ?? new List<Type>()
            };

            return View(model);
        }
    }
}