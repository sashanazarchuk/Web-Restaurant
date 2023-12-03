using AutoMapper;
using BusinessLogic.DTOs.ProductDto;
using BusinessLogic.DTOs.ProductsDto;
using BusinessLogic.Interfaces.ProductServices;
using Entities.Data;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.ProductServices
{
    public class ProductService : IProductService<ProductDto>
    {

        private readonly ResDbContext context;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;

        public ProductService(ResDbContext context, IMapper mapper, ILoggerManager logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProduct()
        {
            try
            {
                var product = await context.Products.Include(p => p.Images.Where(i => i.Priority == 1)).ToListAsync(); // Retrieving the list of products with main product image from the database.

                logger.LogInfo("Product list showed successfully");

                // Transforming and returning the list of products in DTO format.
                return mapper.Map<IEnumerable<ProductDto>>(product);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError("An error occurred while updating the database" + ex.Message);
                throw;// Propagating the exception for further handling.
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError("An error occurred due to an invalid operation" + ex.Message);
                throw;// Propagating the exception for further handling.
            }
            catch (Exception ex)
            {
                logger.LogError("An unexpected error occurred during showing product list" + ex.Message);
                throw;// Propagating the exception for further handling.
            }
            
        }

        public async Task<ProductDto> GetById(int id)
        {
            // Retrieve a product by its unique identifier 
            var product = await context.Products.Include(i => i.Images).FirstOrDefaultAsync(c => c.Id == id);

            if (product == null) return null; // If the product is not found, return null.

            return mapper.Map<ProductDto>(product); // Map and return the product data as a ProductDto.
        }
    }
}
