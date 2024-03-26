using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NanoERP.API.Domain.Entities;
using NanoERP.API.Services;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api/partners")]
    public class PartnerController(PartnerService service) : ControllerBase
    {
        private readonly PartnerService _service = service;

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

        [HttpPost("{id}/addresses")]
        [Authorize]
        public async Task<ActionResult> AddAddress(string id, PartnerAddress address)
        {
            var partner = await _service.GetByIdAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            var created = await _service.AddAddressAsync(partner, address);
            return CreatedAtAction(nameof(GetAddress), new { id, addressId = created.Id }, created);        
        }

        [HttpDelete("{id}/addresses/{addressId}")]
        [Authorize]
        public async Task<ActionResult> RemoveAddress(string id, string addressId)
        {
            var partner = await _service.GetByIdAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            await _service.RemoveAddressAsync(partner, addressId);
            return NoContent();
        }

        [HttpGet("{id}/addresses")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PartnerAddress>>> GetAddresses(string id)
        {
            var partner = await _service.GetByIdAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            return Ok(partner.Addresses);
        }

        [HttpGet("{id}/addresses/{addressId}")]
        [Authorize]
        public async Task<ActionResult<PartnerAddress>> GetAddress(string id, string addressId)
        {
            var partner = await _service.GetByIdAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            var address = partner.Addresses?.FirstOrDefault(a => a.Id.ToString() == addressId);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }
    }
}