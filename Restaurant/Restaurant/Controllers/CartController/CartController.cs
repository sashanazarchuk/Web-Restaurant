using BusinessLogic.Interfaces.ICartService;
using Entities.Models.Entities;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Controllers.CartController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService<CartItem> service;
        private readonly IHttpContextAccessor httpContext;
        private readonly ILoggerManager logger;

        public CartController(ICartService<CartItem> service, IHttpContextAccessor httpContext, ILoggerManager logger)
        {
            this.service = service;
            this.httpContext = httpContext;
            this.logger = logger;
        }


        [HttpPost("AddToCart/{id}")]
        public async Task<IActionResult> AddToCart([FromRoute] int id)
        {
            try
            {
                // Log information about the start of the AddToCart method
                logger.LogInfo($"Start AddToCart method for product ID {id}");

                await service.AddToCart(id);

                // Log information about the successful completion of the AddToCart method
                logger.LogInfo($"Product ID {id} added to the cart successfully");

                return Ok();
            }
            catch (Exception ex)
            {
                // Log an error message if an exception occurs during AddToCart
                logger.LogError($"Error adding product to cart. Error details: {ex.Message}");

                string errorMessage = "Error adding product to cart";
                return BadRequest(errorMessage);
            }
        }


        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = httpContext.HttpContext.User.FindFirst("id")?.Value;
            if (userId != null)
            {
                try
                {
                    // Log information about the start of the ClearCart method
                    logger.LogInfo($"Start ClearCart method for user ID {userId}");

                    await service.ClearCart(userId);

                    // Log information about the successful completion of the ClearCart method
                    logger.LogInfo($"Cart cleared successfully for user ID {userId}");

                    return Ok();
                }
                catch (Exception ex)
                {
                    // Log an error message if an exception occurs during ClearCart
                    logger.LogError($"Error clearing the cart. Error details: {ex.Message}");

                    string errorMessage = "Error clearing the cart";
                    return BadRequest(errorMessage);
                }
            }

            // Log a warning if the user is not found
            logger.LogWarn("User not found");
            return BadRequest("User not found");
        }


        [HttpGet("GetCarts")]
        public IActionResult GetCarts()
        {
            var userId = httpContext.HttpContext.User.FindFirst("id")?.Value;
            if (userId != null)
            {
                try
                {
                    // Log information about the start of the GetCarts method
                    logger.LogInfo($"Start GetCarts method for user ID {userId}");

                    var cartItems = service.GetCartItems(userId);

                    // Log information about the successful completion of the GetCarts method
                    logger.LogInfo($"Cart items retrieved successfully for user ID {userId}");

                    return Ok(cartItems);
                }
                catch (Exception ex)
                {
                    // Log an error message if an exception occurs during GetCarts
                    logger.LogError($"Error showing the cart list. Error details: {ex.Message}");

                    string errorMessage = "Error showing the cart list";
                    return BadRequest(errorMessage);
                }
            }

            // Log a warning if the user is not found
            logger.LogWarn("User not found");
            return BadRequest("User not found");
        }


        [HttpGet("GetTotal")]
        public IActionResult GetTotalPrice()
        {
            var userId = httpContext.HttpContext.User.FindFirst("id")?.Value;
            if (userId != null)
            {
                try
                {
                    // Log information about the start of the GetTotalPrice method
                    logger.LogInfo($"Start GetTotalPrice method for user ID {userId}");

                    var totalPrice = service.GetTotalPrice(userId);

                    // Log information about the successful completion of the GetTotalPrice method
                    logger.LogInfo($"Total price retrieved successfully for user ID {userId}");

                    return Ok(totalPrice);
                }
                catch (Exception ex)
                {
                    // Log an error message if an exception occurs during GetTotalPrice
                    logger.LogError($"Error showing the total price. Error details: {ex.Message}");

                    string errorMessage = "Error showing the total price";
                    return BadRequest(errorMessage);
                }
            }

            // Log a warning if the user is not found
            logger.LogWarn("User not found");
            return BadRequest("User not found");
        }


        [HttpDelete("RemoveItem/{productId}")]
        public async Task<IActionResult> GetRemoveItem([FromRoute] int productId)
        {
            var userId = httpContext.HttpContext.User.FindFirst("id")?.Value;
            if (userId != null)
            {
                try
                {
                    // Log information about the start of the RemoveItem method
                    logger.LogInfo($"Start RemoveItem method for product ID {productId} and user ID {userId}");

                    await service.RemoveItem(productId, userId);

                    // Log information about the successful completion of the RemoveItem method
                    logger.LogInfo($"Product ID {productId} removed from the cart successfully for user ID {userId}");

                    return Ok();
                }
                catch (Exception ex)
                {
                    // Log an error message if an exception occurs during RemoveItem
                    logger.LogError($"Error removing item from the cart. Error details: {ex.Message}");

                    string errorMessage = "Error removing item from the cart";
                    return BadRequest(errorMessage);
                }
            }

            // Log a warning if the user is not found
            logger.LogWarn("User not found");
            return BadRequest("User not found");
        }
    }
}