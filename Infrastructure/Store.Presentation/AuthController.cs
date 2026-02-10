using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result =await serviceManager.AuthService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("Register")] // path : api/Auth/Register
        public async Task<IActionResult> Register(RegisterRequest request)
        {
           var result= await serviceManager.AuthService.RegisterAsync(request);
            return Ok(result);
        }
    }
}
