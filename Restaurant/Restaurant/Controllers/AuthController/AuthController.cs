using BusinessLogic.Interfaces.IAuthServices;
using Entities.Models.Auth;
using Entities.Models.Entities;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoggerManager logger;
        private readonly UserManager<User> userManager;
        private readonly IJwtTokenService service;

        public AuthController(ILoggerManager logger, UserManager<User> userManager, IJwtTokenService service)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.service = service;

        }

        // Handles user login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            // Find the user by email
            var user = await userManager.FindByEmailAsync(model.Email);

            // Check if the user exists
            if (user != null)
            {
                // Check if the provided password is valid
                var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);

                // If the password is not valid, log a warning and return BadRequest
                if (!isPasswordValid)
                {
                    logger.LogWarn($"{nameof(Login)}: Authentication failed. Wrong user name or password.");
                    return BadRequest();
                }

                // If authentication is successful, generate a JWT token for the user
                var token = await service.CreateToken(user);

                // Return the JWT token in the response
                return Ok(new { token });
            }

            // If the user is not found, return BadRequest
            return BadRequest();
        }

        // Handles user logout
        [HttpGet("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Clear the session (if used)
            HttpContext.Session.Clear();
            // Return Ok to indicate successful logout
            return Ok();
        }
    }
}
