using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NanoERP.API.Data;
using NanoERP.API.Domain.Entities;
using NanoERP.API.Services;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(DataContext db) : ControllerBase
    {
        private readonly UserService _userService = new(db);

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_userService.Get());
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<User> GetById(string id)
        {
            var user = _userService.GetById(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [Authorize]
        [Conditional("DEBUG")]
        [HttpGet("truncate")]
        public void TruncateUsers_DevelopmentOnly()
        {
            _userService.Truncate();
        }
    }
}