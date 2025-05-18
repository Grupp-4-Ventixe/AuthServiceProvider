using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class SignInDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
