using System.ComponentModel.DataAnnotations;
using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Data.Rules.ValidationRules;
using BeestjeOpJeFeestje.Repository.Enums;

namespace BeestjeOpJeFeestje.Models.Users;

public class SingleUserViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public Rank Rank { get; set; }

    [Required]
    [PostalCodeRule]
    public string ZipCode { get; set; }

    [Required]
    [StringLength(5)]
    public string HouseNumber { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    public bool Check { get; set; }
    public string? Result { get; set; }


    public UserDto ToDto()
    {
        return new UserDto()
        {
            Name = this.Name,
            Email = this.Email,
            Rank = this.Rank,
            HouseNumber = this.HouseNumber,
            ZipCode = this.ZipCode,
            PhoneNumber = this.PhoneNumber
        };
    }
}