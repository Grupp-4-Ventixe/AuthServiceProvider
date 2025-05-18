
using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace Business.Services;


public class AuthService : IAuthService
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignUpResult> SignUpAsync(SignUpDto formData)
    {
        var user = new IdentityUser
        {
            UserName = formData.Email,
            Email = formData.Email
        };

        var result = await _userManager.CreateAsync(user, formData.Password);

        return new SignUpResult
        {
            Succeeded = false,
            Errors = new List<string>()

        };
    }
    public async Task<string> SignInAsync(SignInDto formData)
    {
        var result = await _signInManager.PasswordSignInAsync(
            formData.Email,
            formData.Password,
            formData.RememberMe,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        using var httpClient = new HttpClient();
        var tokenResponse = await httpClient.PostAsJsonAsync("https://tokenservice.azurewebsites.net/api/token", new
        {
            Email = formData.Email
        });

        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new Exception("Failed to retrieve JWT token from the external service.");
        }

        return await tokenResponse.Content.ReadAsStringAsync();

    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.Email == email);
    }


}

