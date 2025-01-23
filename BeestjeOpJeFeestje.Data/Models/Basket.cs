using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Data.Models;

public class Basket
{
    public List<ProductDto> Products { get; set; } = new List<ProductDto>();
}