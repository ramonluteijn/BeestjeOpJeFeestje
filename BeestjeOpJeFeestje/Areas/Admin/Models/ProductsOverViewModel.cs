using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Areas.Admin.Models;

public class ProductsOverViewModel
{
    public IEnumerable<ProductDto> Products { get; set; }
    public List<Type> SelectedTypes { get; set; } = new List<Type>();
    public int BasketCount { get; set; }
}