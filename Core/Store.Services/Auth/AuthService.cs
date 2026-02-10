using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Domain.Entities.Identity;
using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Domain.Exceptions.Unauthorized;
using Store.Services.Abstractions.Auth;
using Store.Shared;
using Store.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Auth
{
    public class AuthService(UserManager<AppUser> userManager,IOptions<JwtOptions> options) : IAuthService
    {
        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
           var user=await userManager.FindByEmailAsync(request.Email);

            if (user is null) throw new UserNotFoundException(request.Email);

           var flag= await userManager.CheckPasswordAsync(user, request.Password);

            if (!flag) throw new UnauthorizedException();

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser()
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                UserName = request.Username,
                PhoneNumber = request.PhoneNumber
            };

             var result=await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new RegisterationBadRequestException(result.Errors.Select(s=>s.Description).ToList());

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            // Claims are pieces of information about the user 
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber)
            };

            //  retrieves the roles associated with a user.
            var userRoles = await userManager.GetRolesAsync(user);

            // adding roles as claims to the token. 
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var JwtOptions = options.Value;
            // The key is a secret value that is used to sign the token. it must be long and complex enough to ensure the security of the token.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));

            // The token is created using the JwtSecurityToken class, which takes several parameters:
            var token = new JwtSecurityToken(

                //Who issued the token (the server)
                issuer: JwtOptions.issuer,
                // Who is allowed to use the token
                audience: JwtOptions.audience,
                // the claims are included information about user
                claims: authClaims,
                //Token expiration duration
                expires: DateTime.Now.AddDays(JwtOptions.DurationDays),
                //Signing algorithm
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            // convert token to string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
