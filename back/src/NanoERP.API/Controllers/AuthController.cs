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
    [Route("api")]
    public class AuthController(IConfiguration configuration, UserService userService, EmailService emailService) : ControllerBase
    {
        private readonly UserService _userService = userService;
        private readonly IConfiguration _configuration = configuration;
        private readonly EmailService _emailService = emailService; 

        [HttpPost("login")]
        public IActionResult Auth(UserLoginDto userLogin)
        {
            var user = _userService.FindByUsernameOrEmail(userLogin);

            if (user != null && VerifyUserPassword(userLogin.Password, user.Password))
            {
                var token = TokenService.Generate(_configuration["Jwt:Key"] ?? "", user);

                return Ok(new { Token = token });
            }

            return BadRequest("Invalid username or password");
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserRegistrationDto bodyUser)
        {
            var existingUser = _userService.FindByUsernameOrEmail(bodyUser);

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

            return Created("User created successfully", newUser);
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword(UserChangePasswordDto userChangePassword)
        {
            var user = _userService.FindByUsernameOrEmail(userChangePassword);

            if (user != null && VerifyUserPassword(userChangePassword.Password, user.Password))
            {
                if (userChangePassword.NewPassword == userChangePassword.ConfirmPassword)
                {
                    user.Password = userChangePassword.NewPassword;

                    _userService.Update(user);

                    return Ok("Password changed successfully");
                }

                return BadRequest("The new password and confirm password do not match");
            }

            return BadRequest("Invalid username or password");
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(UserForgotPasswordDto userForgotPassword)
        {
            var user = _userService.FindByEmail(userForgotPassword.Email);

            if (user != null)
            {
                var newPassword = PasswordService.GenerateRandomPassword();

                user.Password = EncryptionService.Hash(newPassword);

                _userService.Update(user);

                _emailService.Send(userForgotPassword.Email, "Password recovery", $"Your new password is: {newPassword}");

                return Ok("New password sent to your email");
            }

            return BadRequest("Email is not registered");
        }

        private static bool VerifyUserPassword(string input, string target)
        {
            return EncryptionService.Verify(input, target);
        }
    }
}