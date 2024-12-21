using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Data.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Type Type { get; set; }
    public int Price { get; set; }
    public string Img { get; set; }
    public bool IsInBasket { get; set; }
}