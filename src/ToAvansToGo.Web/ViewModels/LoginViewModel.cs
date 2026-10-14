using System.ComponentModel.DataAnnotations;

namespace ToAvansToGo.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vul je e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je wachtwoord in.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}