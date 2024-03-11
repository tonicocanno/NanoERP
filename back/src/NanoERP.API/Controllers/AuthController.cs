using Microsoft.AspNetCore.Mvc;
using NanoERP.API.Data;
using NanoERP.API.Domain.Entities.DTO;
using NanoERP.API.Services;
using NanoERP.API.Utilities;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController(DataContext db, IConfiguration configuration) : ControllerBase
    {
        private readonly UserService _userService = new(db);
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("login")]
        public ActionResult Auth(UserLoginDto userLogin)
        {
            var user = _userService.GetByUsernameOrEmail(userLogin);

            if (user != null && EncryptionService.Verify(userLogin.Password, user.Password))
            {
                var token = TokenService.Generate(_configuration["Jwt:Key"] ?? "", user);

                return Ok(new { Token = token });
            }

            return BadRequest("Invalid username or password");
        }
    }
}