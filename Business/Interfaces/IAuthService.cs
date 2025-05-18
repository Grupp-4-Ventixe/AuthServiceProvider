using Business.Dtos;

namespace Business.Interfaces;

public interface IAuthService
{
    Task LogoutAsync();
    Task<string> SignInAsync(SignInDto formData);
    Task<SignUpResult> SignUpAsync(SignUpDto formData);
    Task<bool> UserExistsAsync(string email);
}