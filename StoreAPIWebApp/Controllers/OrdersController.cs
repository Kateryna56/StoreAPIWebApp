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
    public class OrdersController : ControllerBase
    {
        private readonly StoreAPIContext _context;

        public OrdersController(StoreAPIContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<Order>>> GetOrders([FromQuery] Parameters parameters)
        {
            var query = _context.Orders.AsQueryable();

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
                nextLink = Url.Action("GetOrders", null, new { PageNumber = nextPageNumber, PageSize = pageSize }, Request.Scheme);
            }

            // Створюємо об'єкт результата
            var result = new PaginatedResult<Order>
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


        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'StoreAPIContext.Orders'  is null.");
          }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
