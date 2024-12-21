using Type = BeestjeOpJeFeestje.Repository.Enums.Type;

namespace BeestjeOpJeFeestje.Repository.Interfaces;

public interface IImageFactory
{
    string GetRandomImageByCategory(Type type);
}