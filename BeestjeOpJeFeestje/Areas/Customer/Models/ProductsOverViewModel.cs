using BeestjeOpJeFeestje.Data.Dtos;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Areas.Customer.Models;

public class ProductsOverViewModel
{
    public IEnumerable<ProductDto> Products { get; set; }
    public List<Type> SelectedTypes { get; set; } = new List<Type>();
    public int BasketCount { get; set; }

    public int GetTotallPrice()
    {
        return Products
            .Where(p => p.IsInBasket)
            .Sum(p => p.Price);
    }
}