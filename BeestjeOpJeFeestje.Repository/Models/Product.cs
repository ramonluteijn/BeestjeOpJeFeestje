using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Repository.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public Type Type { get; set; }
    public int Price { get; set; }
    public string Img { get; set; }
}