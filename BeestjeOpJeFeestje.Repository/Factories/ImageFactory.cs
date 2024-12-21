using BeestjeOpJeFeestje.Repository.Interfaces;
using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Repository.Factories;

public class ImageFactory : IImageFactory
{
    public string GetRandomImageByCategory(Type type)
    {
        string[] images = type switch
        {
            Type.FARM => new[] { "chicken.png", "cow.png", "dog.png", "donkey.png", "rubber-duck.png" },
            Type.DESERT => new[] { "" }, // Add images for desert animals
            Type.JUNGLE => new[] { "elephant.png", "lion.png", "monkey.png", "zebra" },
            Type.SNOW => new[] { "penguin" }, // Add images for snow animals
            Type.VIP => new[] { "" }, // Add images for VIP animals
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        return images[new Random().Next(images.Length)];
    }
}