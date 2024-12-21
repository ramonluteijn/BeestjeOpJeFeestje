using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Areas.Admin.Models;

public class ProductsOverViewModel
{
    public IEnumerable<ProductDto> products { get; set; }

}