using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] ProductWithImageDto dto)
        {
            
            // Use the ImagePath from the DTO as the image path (no need to handle file upload here)
            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                ProductionDate = dto.ProductionDate,
                UserId = dto.UserId,
                ImagePath = dto.Image, // Directly use the path provided by the MVC app
                Price = dto.Price,       // Now assigning the Price
                Description = dto.Description
            };

            // Save the product to the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            Console.WriteLine("Product saved to DB.");

            return Ok("Product added successfully.");
        }



        [HttpGet("my-products")]
        //[Authorize(Roles = "Farmer")]
        public IActionResult GetMyProducts()
        {
            
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"The user id is: {userId}");
            var products = _context.Products
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Category,
                    p.ProductionDate,
                    p.ImagePath,
                    p.UserId,
                    p.Price,
                    p.Description
                })
                .ToList();
            return Ok(products);
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? category, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? farmerId) 
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= toDate.Value);
            }

            // Filter by FarmerId if it's provided
            if (!string.IsNullOrEmpty(farmerId))
            {
                query = query.Where(p => p.UserId == farmerId);
            }

            var products = await query
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Category,
                    p.ProductionDate,
                    p.ImagePath,
                    p.UserId,
                    p.Price,
                    p.Description
                })
                .ToListAsync();

            return Ok(products);
        }







    }
}
