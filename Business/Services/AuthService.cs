
using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Http.Json;
using System.Text.Json;

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
            Succeeded = result.Succeeded,
            Errors = result.Errors.Select(e => e.Description).ToList(),
            Message = result.Succeeded ? "User created successfully." : "User creation failed."
        };
    }
    public async Task<(string Token, string Email, string Name, string Role)> SignInAsync(SignInDto formData)
    {
      
        var result = await _signInManager.PasswordSignInAsync(
            formData.Email,
            formData.Password,
            formData.RememberMe,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials.");

  
        var user = await _userManager.FindByEmailAsync(formData.Email)
            ?? throw new UnauthorizedAccessException("User not found.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Member";

      
        using var httpClient = new HttpClient();

        var tokenRequest = new
        {
            UserId = user.Id,
            Role = role
        };

        var response = await httpClient.PostAsJsonAsync(
            "https://tokenservice-hghjfwgwf9cubxdp.swedencentral-01.azurewebsites.net/auth/token",
            tokenRequest
        );

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to retrieve JWT token from the external service.");

  
        var json = await response.Content.ReadAsStringAsync();
        var tokenObj = JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (tokenObj?.Token == null)
            throw new Exception("Token was null after deserialization.");

    
        return (
            Token: tokenObj!.Token,
            Email: user.Email!,
            Name: user.UserName!,
            Role: role
        );
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

