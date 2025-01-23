using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BeestjeOpJeFeestje.Data.Rules.ValidationRules;

public class PostalCodeRule : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) return false;
        var postalCode = value.ToString();
        return Regex.IsMatch(postalCode, @"^\d{4}[A-Za-z]{2}$"); // 1234AB
    }
}