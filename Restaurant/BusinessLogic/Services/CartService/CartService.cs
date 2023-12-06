using BusinessLogic.Interfaces.ICartService;
using Entities.Data;
using Entities.Models.Entities;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.CartService
{
    public class CartService : ICartService<CartItem>
    {
        private readonly ResDbContext context;
        private readonly IHttpContextAccessor httpContext;
        private readonly ILoggerManager logger;

        public CartService(ResDbContext context, IHttpContextAccessor httpContext, ILoggerManager logger)
        {
            this.context = context;
            this.httpContext = httpContext;
            this.logger = logger;
        }

        public async Task AddToCart(int productId)
        {
            // Get the user's ID from the HTTP context
            var userId = httpContext.HttpContext.User.FindFirst("id")?.Value;

            if (userId != null)
            {
                // Check if the product is already in the user's cart
                var cartItem = await context.CartItem
                    .SingleOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

                if (cartItem == null)
                {
                    // If the product is not in the cart, create a new cart item
                    cartItem = new CartItem
                    {
                        UserId = userId,
                        ProductId = productId,
                        Product = context.Products.SingleOrDefault(p => p.Id == productId),
                        Quantity = 1
                    };
                    
                    await context.CartItem.AddAsync(cartItem);

                    //New item added to the cart
                    logger.LogInfo($"User {userId} added a new item (ID: {productId}) to the cart.");
                }
                else
                {
                    // If the product is already in the cart, increment the quantity
                    cartItem.Quantity++;

                    //Item quantity incremented in the cart
                    logger.LogInfo($"User {userId} incremented the quantity of item (ID: {productId}) in the cart.");
                }
                // Save changes to the database
                await context.SaveChangesAsync();
            }
        }

        
        public async Task ClearCart(string userId)
        {
            // Get all cart items associated with the user
            var cartItem = await context.CartItem.Where(u => u.UserId == userId).ToListAsync();

            // Remove all cart items
            context.CartItem.RemoveRange(cartItem);

            //Cart cleared for the user
            logger.LogInfo($"User {userId}'s cart has been cleared.");

            // Save changes to the database
            await context.SaveChangesAsync();
        }


        public IEnumerable<CartItem> GetCartItems(string userId)
        {
            // Retrieve all cart items for the user, including product details
            var cartItems = context.CartItem.Include(c => c.Product).Where(c => c.UserId == userId).ToList();

            //Cart items retrieved for the user
            logger.LogInfo($"Retrieved cart items for user {userId}.");

            return cartItems;
        }


        public decimal GetTotalPrice(string userId)
        {
            // Calculate the total price of items in the user's cart
            decimal total = context.CartItem
                .Where(cart => cart.UserId == userId)
                .Sum(cart => cart.Quantity * cart.Product.Price);

            //Total price calculated for the user's cart
            logger.LogInfo($"Total price calculated for user {userId}'s cart: {total:C}");

            return total;
        }


        public async Task RemoveItem(int productId, string userId)
        {
            // Find the cart item associated with the user and product
            var cartItem = await context.CartItem.SingleOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                // Remove the cart item
                context.CartItem.Remove(cartItem);
                
                //Item removed from the cart
                logger.LogInfo($"Item (ID: {productId}) removed from the cart for user {userId}.");
                
                // Save changes to the database
                await context.SaveChangesAsync();
            }
        }
    }
}
