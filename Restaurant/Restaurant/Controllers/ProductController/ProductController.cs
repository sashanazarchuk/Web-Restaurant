using BusinessLogic.DTOs.ProductDto;
using BusinessLogic.DTOs.ProductsDto;
using BusinessLogic.Interfaces.ProductServices;
using Entities.Models.Entities;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Controllers.ProductController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService<ProductDto> service;
        private readonly ILoggerManager logger;

        public ProductController(IProductService<ProductDto> service, ILoggerManager logger)
        {
            this.service = service;
            this.logger = logger;
        }


        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var result = await service.GetAllProduct();
            return Ok(result);
            //try
            //{
            //    var result = await service.GetAllProduct(); // Retrieve a list of all products from product service

            //    // Check if the result is null
            //    if (result == null)
            //    {
            //        logger.LogError("A NullReferenceException occurred while receiving the products");
            //        return BadRequest("A NullReferenceException occurred while receiving the products"); // Returning a bad request with an error message.
            //    }

            //    logger.LogInfo("Product list return successfully");
            //    return Ok(result);// Returning a successful response with the product list.
            //}
            //catch (Exception ex)
            //{

            //    logger.LogError("An error occurred while receiving the products: " + ex.Message);
            //    return BadRequest("An error occurred while receiving the products"); // Returning a bad request with an error message.
            //}
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await service.GetById(id);
                if (result == null) return NotFound(); // Product not found, return 404
                return Ok(result); // Return the product data as OK (200) response
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred while receiving the current product: " + ex.Message);
                return BadRequest("An error occurred while receiving the current product"); // Return a Bad Request (400) response in case of an exception
            }
        }
    }
}
