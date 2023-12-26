using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPIWebApp.Models;

namespace StoreAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreAPIContext _context;

        public ProductsController(StoreAPIContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<Product>>> GetProducts([FromQuery] Parameters parameters)
        {
            var query = _context.Products.AsQueryable();

            // Проводимо пагінацію
            var totalCount = await query.CountAsync();
            var pageSize = parameters.PageSize;
            var pageNumber = parameters.PageNumber;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Створюємо посилання на наступну сторінку, якщо вона існує
            string nextLink = null;
            if (pageNumber < totalPages)
            {
                var nextPageNumber = pageNumber + 1;
                nextLink = Url.Action("GetProducts", null, new { PageNumber = nextPageNumber, PageSize = pageSize }, Request.Scheme);
            }

            // Створюємо об'єкт результата
            var result = new PaginatedResult<Product>
            {
                NextLink = nextLink,
                Values = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalPages = totalPages
            };

            return Ok(result);
        }


        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'StoreAPIContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
