using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NanoERP.API.Domain.Entities;
using NanoERP.API.Services;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController(ProductService service) : ControllerBase
    {
        private readonly ProductService _service = service;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var Product = await _service.GetByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Ok(Product);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> Post(Product Product)
        {
            Product.Id = ObjectId.GenerateNewId();
            var created = await _service.CreateAsync(Product);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            var Product = await _service.GetByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(Product);
            return NoContent();
        }
    }
}