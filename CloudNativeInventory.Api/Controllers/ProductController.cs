using Microsoft.AspNetCore.Mvc;
using CloudNativeInventory.Api.DTOS;
using CloudNativeInventory.Api.Services;


namespace CloudNativeInventory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            try
            {
                var result = await _service.CreateProductAsync(createProductDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteProduct([FromBody] Guid id)
        {
            try
            {
                var result = await _service.DeleteProductAsync(id);
                if (!result) return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
