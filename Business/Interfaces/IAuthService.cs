using Business.Dtos;

namespace Business.Interfaces;

public interface IAuthService
{
    Task LogoutAsync();
    Task<(string Token, string Email, string Name, string Role)> SignInAsync(SignInDto formData);
    Task<SignUpResult> SignUpAsync(SignUpDto formData);
    Task<bool> UserExistsAsync(string email);
}