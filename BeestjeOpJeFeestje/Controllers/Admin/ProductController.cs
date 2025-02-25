﻿using BeestjeOpJeFeestje.Data.Services;
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
        var productsOverviewModel = new ProductsOverViewModel { Products = products };
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
        await HandleProductSave(productViewModel, true);
        return View(productViewModel);
    }

    private SingleProductViewModel GetProductViewModel(int id)
    {
        var product = productService.GetProductById(id);
        return new SingleProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Type = product.Type,
            Img = product.Img,
            Orders = orderService.GetAllOrdersByProductId(id)
        };
    }

    [HttpGet("{id:int}/details")]
    public IActionResult Details(int id)
    {
        var productViewModel = GetProductViewModel(id);
        return View(productViewModel);
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        var productViewModel = GetProductViewModel(id);
        return View(productViewModel);
    }

    [HttpPost("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id, SingleProductViewModel productViewModel)
    {
        productViewModel = await HandleProductSave(productViewModel, false, id);
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

    private async Task<SingleProductViewModel> HandleProductSave(SingleProductViewModel productViewModel, bool isCreate, int? id = null)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var productDto = productViewModel.ToDto();
                (bool check, string result, string img) operationResult = isCreate ? productService.CreateProduct(productDto) : productService.UpdateProduct(id.Value, productDto);
                productViewModel.Check = operationResult.check;
                productViewModel.Result = operationResult.result;
                productViewModel.Img = operationResult.img;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
            }
        }

        return productViewModel;
    }
}