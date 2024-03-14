using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NanoERP.API.Domain.Entities;
using NanoERP.API.Services;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api/partners")]
    public class PartnerController(PartnerService partnerService) : ControllerBase
    {
        private readonly PartnerService _service = partnerService;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Partner>>> Get()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Partner>> Get(string id)
        {
            var partner = await _service.GetByIdAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            return Ok(partner);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Partner>> Post(Partner partner)
        {
            partner.Id = ObjectId.GenerateNewId();
            var created = await _service.CreateAsync(partner);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            var partner = await _service.GetByIdAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(partner);
            return NoContent();
        }
    }
}