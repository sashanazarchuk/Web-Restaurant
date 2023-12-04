using BusinessLogic.Interfaces.IAuthService;
using Entities.Models.Auth;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService<IdentityResult> service;
        private readonly ILoggerManager logger;
        public RegisterController(IRegisterService<IdentityResult> service, ILoggerManager logger)
        {
            this.service = service;
            this.logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                // Log information about the registration process
                logger.LogInfo("Registering a new user...");

                // Call the registration service to attempt user registration
                var result = await service.Register(model);

                // Check if the registration was not successful
                if (!result.Succeeded)
                {
                    // Iterate through the errors and add them to the ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }

                    // Log a warning that the registration failed due to a bad request
                    logger.LogWarn("Registration failed. Bad request.");

                    // Return a BadRequest response with the ModelState containing error details
                    return BadRequest(ModelState);
                }

                // Log information about the successful registration
                logger.LogInfo("Registration successful.");

                // Return a StatusCode 201 (Created) response for successful registration
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                // Log an error message if an exception occurs during user registration
                logger.LogError($"An error occurred during user registration: {ex.Message}");

                // Return a StatusCode 500 (Internal Server Error) response for unexpected errors
                return StatusCode(500);
            }
        }
    }
}
