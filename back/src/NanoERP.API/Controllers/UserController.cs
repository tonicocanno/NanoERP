using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NanoERP.API.Data;
using NanoERP.API.Domain.Entities;
using NanoERP.API.Domain.Entities.DTO;
using NanoERP.API.Services;
using NanoERP.API.Utilities;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(DataContext db) : ControllerBase
    {
        private readonly UserService _userService = new(db);

        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] UserRegistrationDto bodyUser)
        {
            var existingUser = _userService.GetByUsernameOrEmail(bodyUser);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            bodyUser.Password = EncryptionService.Hash(bodyUser.Password);

            var newUser = new User
            {
                Id = ObjectId.GenerateNewId(),
                Name = bodyUser.Name,
                Surname = bodyUser.Surname,
                Email = bodyUser.Email,
                Username = bodyUser.Username,
                Password = bodyUser.Password
            };

            _userService.Create(newUser);

            return Ok(newUser);
        }

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