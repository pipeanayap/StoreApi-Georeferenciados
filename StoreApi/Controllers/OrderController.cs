using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Models.DTOs;
using StoreApi.Models.Entities;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public OrderController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {
            var orders = await _context.Order
                .Include(o => o.SystemUser)
                .ToListAsync();
            return Ok(orders);
        }
        
        
        
        
        
        
        [HttpPost]
        public async Task<ActionResult> CreateOrder(
            [FromBody] OrderCDTO order)
        {   
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newOrder = new Order
                {
                    SystemUserId = order.SystemUserId,
                    CreatedAt = DateTime.Now,
                    Total = order.Total
                };       
                _context.Order.Add(newOrder);
                await _context.SaveChangesAsync();
                // Insertar en order product
                var orderProducts = order.Products
                    .Select(x => new OrderProduct{OrderId = newOrder.Id, ProductId = x, Amount = 3})
                    .ToList(); 
                _context.OrderProducts.AddRange(orderProducts);
                await _context.SaveChangesAsync();
                //Commit de la transacción
                await transaction.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                
                // Rollback por si sale mal la transacción
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                throw;
            }
           
        }
    }
}
