using BusinessLogic.Interfaces.IAuthService;
using Entities.Data;
using Entities.Models.Auth;
using Entities.Models.Entities;
using Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.AuthServices
{
    public class RegisterService : IRegisterService<IdentityResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ILoggerManager logger;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterService(UserManager<User> userManager, ILoggerManager logger, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> Register(RegisterViewModel model)
        {
            try
            {
                // Check if the entered email is not already in use
                var existingUser = await userManager.FindByEmailAsync(model.Email);

                // If the email is already registered, return a failed IdentityResult
                if (existingUser != null)
                {
                    logger.LogWarn($"Registration failed. Email '{model.Email}' is already registered.");
                    return IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email is already registered." });
                }

                // Create a new user
                var user = new User
                {
                    Name = model.UserName,
                    UserName = model.Email,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Password = model.Password
                };

                // Attempting to create a user
                var result = await userManager.CreateAsync(user, model.Password);

                 //If succeeded added user role
                if (result.Succeeded)
                {
                    // Log success or errors based on the result of user creation 
                    logger.LogInfo($"Registration successful for user '{model.Email}'.");

                    //Add user role "User"
                    var userRole = await roleManager.FindByNameAsync("User");
                    if (userRole != null)
                    {
                        await userManager.AddToRoleAsync(user, userRole.Name);
                    }
                }
                else
                    logger.LogError($"Registration failed. Errors: {string.Join(", ", result.Errors)}");

                // Return the IdentityResult indicating the success or failure of the registration
                return result;
            }
            catch (Exception ex)
            {
                // Log an error message if an exception occurs during user registration and rethrow the exception
                logger.LogError($"An error occurred during registration: {ex.Message}");
                throw;
            }
        }
    }
}