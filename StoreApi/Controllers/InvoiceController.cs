using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using OpenAI.Chat;
using StoreApi.Migrations;
using StoreApi.Models.DTOs;
using StoreApi.Models.Entities;
using Invoice = StoreApi.Models.Entities.Invoice;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        
        private readonly StoreDbContext _context;
        private readonly IConfiguration _config;
        
        public InvoiceController(StoreDbContext context, IConfiguration config)
        {
            _context = context;
            _config =  config;
        }
        
        // GET /api/invoices → lista todas las facturas (con filtros opcionales orderId, isPaid).
        [HttpGet]
        public async Task<ActionResult> GetInvoices([FromQuery] int? orderId, [FromQuery] bool? isPaid)
        {
            var query = _context.Invoice.AsQueryable();
        
            if (orderId.HasValue)
                query = query.Where(i => i.OrderId == orderId.Value);
        
            if (isPaid.HasValue)
                query = query.Where(i => i.IsPaid == isPaid.Value);
        
            var invoices = await query.ToListAsync();
            return Ok(invoices);
        }
        
        //GET /api/invoices/{id} → obtiene una factura por Id.
            [HttpGet("{id}")]
            public async Task<ActionResult> GetById(int id)
            {
                var invoice = await _context.Invoice
                    .FirstOrDefaultAsync(i => i.Id == id);
                return Ok(invoice);
            }
            
        // POST /api/invoices → crea una nueva factura (valida datos, calcula Total si no viene).
        [HttpPost]
        public async Task<ActionResult> CreateInvoice([FromBody] Invoice invoice)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newInvoice = new Invoice
                {
                    InvoiceNumber = invoice.InvoiceNumber,
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    Subtotal = invoice.Subtotal,
                    Tax = invoice.Tax,
                    Total = invoice.Subtotal + invoice.Tax,
                    Currency = invoice.Currency,
                    IsPaid = invoice.IsPaid,
                    PaymentDate = invoice.PaymentDate,
                    BillingName = invoice.BillingName,
                    BillingAddress = invoice.BillingAddress,
                    BillingEmail = invoice.BillingEmail,
                    TaxId = invoice.TaxId,
                    OrderId = invoice.OrderId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };             
                _context.Invoice.Add(newInvoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(newInvoice);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                throw;
            }
        }
        
        // POST /api/Invoice/bulk -> crea nuevas facturas, muchas al mismo tiempo en un arreglo 
        [HttpPost("bulk")]
        public async Task<ActionResult> CreateInvoiceBulk([FromBody] List<InvoiceCDTO> invoices)
        {
            if (invoices == null || invoices.Count == 0)
                return BadRequest("No se recibieron facturas");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newInvoices = invoices.Select(o => new Invoice
                {
                    InvoiceNumber = o.InvoiceNumber,
                    IssueDate = o.IssueDate ?? DateTime.Now,
                    DueDate = o.DueDate,
                    Subtotal = o.Subtotal,
                    Tax = o.Tax,
                    // si Invoice.Total es double en la entidad, convertimos desde decimal?
                    Total = o.Total,
                    Currency = o.Currency,
                    IsPaid = o.IsPaid ?? false,
                    PaymentDate = o.PaymentDate,
                    BillingName = o.BillingName,
                    BillingAddress = o.BillingAddress,
                    BillingEmail = o.BillingEmail,
                    TaxId = o.TaxId,
                    OrderId = o.OrderId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }).ToList();

                _context.Invoice.AddRange(newInvoices);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(newInvoices);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
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
            var invoices = await _context.Invoice
                .Include(o => o.Order)
                .ToListAsync();

            var summary = invoices.Select(i => new
            {
                i.Id,
                i.InvoiceNumber,
                i.IssueDate,
                i.DueDate,
                i.Subtotal,
                i.Tax,
                Total = i.Total,
                i.Currency,
                i.IsPaid,
                i.PaymentDate,
                Order = i.Order == null ? null : new
                {
                    i.Order.Id,
                    i.Order.Total,
                    i.Order.CreatedAt
                }
            });
            
            var jsonData = JsonSerializer.Serialize(summary);
            
            //Se hace el prompt
            var prompt = Prompts.GenerateInvoicesPrompt(jsonData);
            
            var result = await client.CompleteChatAsync(
                [new UserChatMessage(prompt)]);
            
            //La ia analiza los datos y responde
            
            // Se da una respuesta con los datos de la IA
            var response = result.Value.Content[0].Text;
            
            return Ok(response);
        }
    }
}
