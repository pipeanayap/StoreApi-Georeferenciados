using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using StoreApi.Models.DTOs;
using StoreApi.Models.Entities;

namespace StoreApi.Controllers
{
    
    
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly StoreDbContext _context;
        
        private readonly IConfiguration _config;

        public OrderController(StoreDbContext context, IConfiguration config)
        {
            _context = context;
            _config =  config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _context.Order
                .Include(o=>o.SystemUser)
                .Select(o => new
                { 
                    Id = o.Id,
                    Total = o.Total,
                    CreatedAt = o.CreatedAt,
                    User = new UserDTO
                    {
                        Id = o.SystemUser.Id,
                        Email = o.SystemUser.Email,
                        FirstName = o.SystemUser.FirstName,
                        LastName = o.SystemUser.LastName,
                    }
                })
                .ToListAsync();
    
            // _context.Order.FirstOrDefaultAsync(o=>o.Id == id);
            return Ok(orders);
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] OrderCDTO order)
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
        
        
        [HttpPost("bulk")]
        public async Task<ActionResult> CreateOrderBulk([FromBody]List<OrderCDTO> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return BadRequest("No se recibieron ordenes");
            }
            //Si yo voy a modificar varias tablas o si muevo muchos registros tenemos que realizar una transaccion
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //convertir lista de OrderDTO en lista de Ordenes ->>> Porque mi dbcontext necesita la entidad de order no de OrderDTO
                //MANERA VIEJA JEJE
                // var newOrders = new List<Order>();
                // foreach (var orderDto in orders)
                // {
                //     var newOrder = new Order();
                //     newOrder.SystemUserId = orderDto.SystemUserId;
                //     newOrder.Total = orderDto.Total;
                //     newOrder.CreatedAt = DateTime.Now;
                //     
                //     newOrders.Add(newOrder);
                // }
                
                //USANDO LINQ
                var newOrders = orders.Select(o => new Order()
                    {
                        SystemUserId = o.SystemUserId,
                        CreatedAt = DateTime.Now,
                        Total = o.Total, 
                        OrderProducts = o.Products.Select(op => new OrderProduct()
                        {
                            Amount = 1, ProductId = op
                        }).ToList()
                    }
                ).ToList();
                
                _context.Order.AddRange(newOrders);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok("Ordenes agregadas ");
            }
            catch (Exception e)
            {
                transaction.RollbackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("ai-analyze")]
        public async Task<ActionResult> AnalyzeOrders()
        {
            //Obtener el APIKey
            var openAIKey = _config["OpenAIKey"];
            var client = new ChatClient(model: "gpt-5-mini", apiKey: openAIKey);
            
            
            
            //Primero se obtienen los datos
            var orders = await _context.Order
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ThenInclude(p => p.Store)
                .ToListAsync();

            var summary = orders.Select(o => new
            {
                o.Id,
                o.Total,
                o.CreatedAt,
                Products = o.OrderProducts.Select(op => new
                {
                    op.Product.Name,
                    op.Product.Price,
                    op.Product.Store.Description
                })
            });
            
            var jsonData = JsonSerializer.Serialize(summary);
            
            //Se hace el prompt
            var prompt = Prompts.GenerateOrdersPrompt(jsonData);
            
            var result = await client.CompleteChatAsync(
                [new UserChatMessage(prompt)]);
            
            //La ia analiza los datos y responde
            
            // Se da una respuesta con los datos de la IA
             var response = result.Value.Content[0].Text;
            
            return Ok(response);
        }
    }
}
