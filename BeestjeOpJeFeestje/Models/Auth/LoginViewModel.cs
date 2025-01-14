using System.ComponentModel.DataAnnotations;

namespace BeestjeOpJeFeestje.Models.Auth;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}