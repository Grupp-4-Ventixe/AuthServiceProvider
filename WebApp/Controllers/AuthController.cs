﻿using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly UserManager<IdentityUser>? _userManager;

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.SignUpAsync(formData);
            return result.Succeeded
                ? Ok(result)
                : Problem(result.Message);

        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var (token, email, name, role) = await _authService.SignInAsync(formData);

                return Ok(new 
                { 
                    Token = token,
                    Email = email,
                    Name = name,
                    Role = role

                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }


    }
}
