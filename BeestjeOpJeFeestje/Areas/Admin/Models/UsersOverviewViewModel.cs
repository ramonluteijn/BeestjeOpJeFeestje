using BeestjeOpJeFeestje.Data.Dtos;

namespace BeestjeOpJeFeestje.Areas.Admin.Models;

public class UsersOverviewViewModel
{
    public IEnumerable<UserDto> customers { get; set; }
}