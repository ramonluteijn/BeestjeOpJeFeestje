using System.ComponentModel.DataAnnotations;
using BeestjeOpJeFeestje.Data.Dtos;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Models.Products;

public class SingleProductViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Price { get; set; }

    [Required]
    public Type Type { get; set; }

    public string? Img { get; set; }

    public bool Check { get; set; }
    public string? Result { get; set; }

    public List<OrderDto>? Orders { get; set; } = new();


    public ProductDto ToDto()
    {
        return new ProductDto()
        {
            Id = Id,
            Name = Name,
            Price = Price,
            Type = Type,
            Img = Img!
        };
    }
}