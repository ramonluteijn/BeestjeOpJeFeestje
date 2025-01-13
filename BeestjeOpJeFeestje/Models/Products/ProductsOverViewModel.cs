using BeestjeOpJeFeestje.Data.Dtos;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Models.Products;

public class ProductsOverViewModel
{
    public IEnumerable<ProductDto> Products { get; set; }
    public List<Type> SelectedTypes { get; set; } = new List<Type>();
    public int BasketCount { get; set; }
}