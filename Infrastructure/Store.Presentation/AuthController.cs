using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost("Login")]  // path : api/Auth/Login
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await serviceManager.AuthService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("Register")] // path : api/Auth/Register
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await serviceManager.AuthService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpGet("CheckEmailExists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var result = await serviceManager.AuthService.CheckEmailExistsAsync(email);
            return Ok(result);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email);

            var result = await serviceManager.AuthService.GetCurrentUserAsync(email.Value);
            return Ok(result);
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var result = await serviceManager.AuthService.GetCurrentUserAddressAsync(email.Value);
            return Ok(result);
        }

        [HttpPut("Address")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUserAddress(AddressDto request)
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var result = await serviceManager.AuthService.UpdateCurrentUserAddressAsync(request, email.Value);
            return Ok(result);
        }
    }
}
