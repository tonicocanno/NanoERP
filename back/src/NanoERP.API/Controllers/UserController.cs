using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanoERP.API.Domain.Entities;
using NanoERP.API.Services;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(UserService service) : ControllerBase
    {
        private readonly UserService _service = service;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(user);
            return NoContent();
        }

        [Authorize]
        [Conditional("DEBUG")]
        [HttpGet("truncate")]
        public async void TruncateUsers_DevelopmentOnly()
        {
            await _service.TruncateAsync();
        }
    }
}