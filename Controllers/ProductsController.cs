using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce.Data;
using E_commerce.Models;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private byte[] ConvertImageToByteArray(IFormFile imageFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                imageFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.OrderBy(p=>p.name).ToListAsync();
        }

        // GET: api/User/{userId}
        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductById(string productId)  // Change to string for userId
        {
            // Use LINQ to find a user by their ID (userId is now string)
            var product = await _context.Product
                .FirstOrDefaultAsync(p => p.productId == productId);  // Compare userId as a string

            if (product == null)
            {
                return NotFound($"User with ID {productId} not found.");
            }

            return product;
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateOrUpdateProduct([FromBody] Product product)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the product already exists by its ID
            var existingProduct = await _context.Product.FindAsync(product.productId);

            if (existingProduct != null)
            {
                // Update the existing product with provided values (and keep existing ones for nulls)
                existingProduct.name = product.name ?? existingProduct.name;
                existingProduct.description = product.description ?? existingProduct.description;
                existingProduct.price = product.price != 0 ? product.price : existingProduct.price;
                existingProduct.category = product.category ?? existingProduct.category;
                existingProduct.stock = product.stock != 0 ? product.stock : existingProduct.stock;
                existingProduct.image = product.image ?? existingProduct.image;

                _context.Product.Update(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(existingProduct); // Return the updated product
            }

            // For new products, assign default values for missing fields
            product.name ??= "Unnamed Product";
            product.description ??= "No description available";
            product.price = product.price != 0 ? product.price : 0.01m; // Minimum price default
            product.category ??= "Uncategorized";
            product.stock = product.stock != 0 ? product.stock : 1; // Minimum stock default

            // Add the new product to the database
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            // Return a 201 response with the URI of the new product
            return CreatedAtAction(nameof(GetProductById), new { productId = product.productId }, product);
        }

        [HttpGet("GetImage/{productId}")]
        public async Task<IActionResult> GetProductImage(int productId)
        {
            var product = await _context.Product.FindAsync(productId);

            if (product == null || product.image == null)
            {
                return NotFound();
            }

            return File(product.image, "image/jpeg"); // Replace "image/jpeg" with the actual MIME type of your images
        }



    }
}
