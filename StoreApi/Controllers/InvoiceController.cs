using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Migrations;
using StoreApi.Models.Entities;
using Invoice = StoreApi.Models.Entities.Invoice;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        
        private readonly StoreDbContext _context;
        
        public InvoiceController(StoreDbContext context)
        {
            _context = context;
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
    }
}
