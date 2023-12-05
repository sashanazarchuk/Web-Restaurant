using BusinessLogic.Interfaces.IAuthServices;
using Entities.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.AuthServices
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;


        public JwtTokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        // Creates a JWT token for the provided user
        public async Task<string> CreateToken(User user)
        {
            // Retrieve the roles assigned to the user using userManager
            IList<string> roles = await userManager.GetRolesAsync(user);

            // Initialize a list to store claims for the JWT token
            List<Claim> claims = new List<Claim>()
            {
                // Add basic claims such as email and user ID
                new Claim("id", user.Id),
                new Claim("name", user.Name),
                new Claim("email",user.Email)
            };

            // Add role claims to the list of claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create a symmetric security key using the JWT secret key from configuration
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<String>("JWTSecretKey")));

            // Create signing credentials using the security key and HMACSHA256 algorithm
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            // Create a JWT token with the specified signing credentials, expiration date, and claims
            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                expires: DateTime.Now.AddDays(1),
                claims: claims
            );

            // Write the JWT token as a string and return it
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }


    }
}
