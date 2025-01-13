using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Models.Users;

public class UsersOverviewViewModel
{
    public IEnumerable<UserDto> customers { get; set; }
}