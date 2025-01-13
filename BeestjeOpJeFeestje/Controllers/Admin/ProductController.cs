using BeestjeOpJeFeestje.Data.Services;
using BeestjeOpJeFeestje.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BeestjeOpJeFeestje.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("/admin/product")]
public class ProductController(ProductService productService, OrderService orderService) : Controller
{
 [HttpGet]
    public IActionResult Index()
    {
        var products = productService.GetProducts();
        var productsOverviewModel = new ProductsOverViewModel
        {
            Products = products
        };
        return View(productsOverviewModel);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(SingleProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var (check, result) = productService.CreateProduct(productViewModel.ToDto());
                productViewModel.Check = check;
                productViewModel.Result = result;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
            }
        }
        return View(productViewModel);
    }


    [HttpGet("{id:int}/details")]
    public IActionResult Details(int id)
    {
        var product = productService.GetProductById(id);
        var productOrders = orderService.GetAllOrdersByProductId(id);
        var productViewModel = new SingleProductViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Type = product.Type,
            Img = product.Img,
            Orders = productOrders
        };
        return View(productViewModel);
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        var product = productService.GetProductById(id);
        var productViewModel = new SingleProductViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Type = product.Type,
            Img = product.Img
        };
        return View(productViewModel);
    }

    [HttpPost("{id:int}/edit")]

    public async Task<IActionResult> Edit(int id, SingleProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var productDto = productViewModel.ToDto();
            try
            {
                var (check, result) = productService.UpdateProduct(id, productDto);
                productViewModel.Check = check;
                productViewModel.Result = result;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
            }
        }
        return View(productViewModel);
    }

    [HttpGet("delete{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await productService.DeleteProduct(id);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
        return RedirectToAction("Index");
    }
}