using K5U1_niklas.Data;
using K5U1_niklas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace K5U1_niklas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly IConfiguration _configuration;

        public InventoryController(InventoryDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Denna endpoint används för att hämta alla produkter i lagret
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // Denna endpoint används för att bevisa att appen framgångsrikt har hämtat den hemliga integrationsnyckeln (från Azure Key Vault i prod)
        [HttpGet("system/verify-integration")]
        public IActionResult VerifyExternalIntegration()
        {
            var apiKey = _configuration["ExternalServices:VendorApiKey"];

            if (string.IsNullOrEmpty(apiKey) || apiKey == "my-vendor-api-key")
            {
                return StatusCode(500, new { Status = "Unsecured", Message = "Körs med lokal (eller saknad) hemlighet!" });
            }

            return Ok(new { Status = "Secured", Message = "Hemlighet laddades framgångsrikt via säker konfiguration." });
        }
    }
}
