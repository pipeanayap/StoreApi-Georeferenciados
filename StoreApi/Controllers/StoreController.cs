using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wkhtmltopdf.NetCore;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly StoreDbContext _context;
        
        private readonly IGeneratePdf _generatePdf;

        public StoreController(IGeneratePdf generatePdf, StoreDbContext context)
        {
            _generatePdf = generatePdf;
            _context = context;
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> Get(int id)
        {
            var store = await _context.Store.
                Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
            var result = await _generatePdf.GetPdf("Templates/StoreTemplate.cshtml", store);
            return result;
        }

    }
}