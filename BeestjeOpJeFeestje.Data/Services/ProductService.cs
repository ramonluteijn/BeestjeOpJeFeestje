using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Repository;
using BeestjeOpJeFeestje.Repository.Factories;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Data.Services;

public class ProductService(MainContext context, ImageFactory imageFactory)
{
    public ProductDto GetProductById(int id)
    {
        var product = context.Products.Find(id);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Type = product.Type,
            Price = product.Price,
            Img = product.Img
        };
    }

    public List<ProductDto> GetProducts(DateOnly? date = null, List<Type>? selectedTypes = null)
    {
        var products = context.Products.
            Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type,
                Price = product.Price,
                Img = product.Img
            }).ToList();

        if (date != null)
        {
            var ordersOnDate = context.Orders
                .Where(order => order.OrderFor == date)
                .Include(order => order.OrderDetails)
                .SelectMany(order => order.OrderDetails)
                .Select(orderDetail => orderDetail.ProductId)
                .ToList();

            products = products.Where(product => !ordersOnDate.Contains(product.Id)).ToList();
        }

        if (selectedTypes != null && selectedTypes.Any())
        {
            products = products.Where(p => selectedTypes.Contains(p.Type)).ToList();
        }

        return products;
    }

    public (bool, string) CreateProduct(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Type = productDto.Type,
            Price = productDto.Price,
            Img = imageFactory.GetRandomImageByCategory(productDto.Type)
        };
        context.Products.Add(product);
        context.SaveChanges();
        return (true, "Product created");
    }

    public (bool,string) UpdateProduct(int id, ProductDto productDto)
    {
        var product = context.Products.Find(id);
        if(product.Type != productDto.Type)
        {
            product.Img = imageFactory.GetRandomImageByCategory(productDto.Type);
        }
        product.Name = productDto.Name;
        product.Type = productDto.Type;
        product.Price = productDto.Price;

        context.SaveChanges();
        return (true, "Product updated");
    }

    public Task DeleteProduct(int id)
    {
        var product = context.Products.Find(id);
        context.Products.Remove(product);
        context.SaveChanges();
        return Task.CompletedTask;
    }
}