using BeestjeOpJeFeestje.Data.Dtos;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Areas.Customer.Models;

public class ProductsOverViewModel
{
    public IEnumerable<ProductDto> Products { get; set; }
    public List<Type> SelectedTypes { get; set; } = new List<Type>();
    public DateOnly Date { get; set; }
    public int BasketCount { get; set; }
}