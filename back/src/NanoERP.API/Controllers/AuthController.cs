using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NanoERP.API.Domain.Entities;
using NanoERP.API.DTO;
using NanoERP.API.Services;
using NanoERP.API.Utilities;

namespace NanoERP.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IConfiguration configuration, UserService userService, EmailService emailService) : ControllerBase
    {
        private readonly UserService _userService = userService;
        private readonly IConfiguration _configuration = configuration;
        private readonly EmailService _emailService = emailService; 

        [HttpPost("login")]
        public async Task<IActionResult> Auth(UserLoginDto userLogin)
        {
            var user = await _userService.FindByUsernameOrEmailAsync(userLogin);

            if (user != null && VerifyUserPassword(userLogin.Password, user.Password))
            {
                var token = TokenService.Generate(_configuration["Jwt:Key"] ?? "", user);

                return Ok(new { Token = token });
            }

            return BadRequest("Invalid username or password");
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegistrationDto bodyUser)
        {
            var existingUser = await _userService.FindByUsernameOrEmailAsync(bodyUser);

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

            await _userService.CreateAsync(newUser);

            return Created("User created successfully", newUser);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDto userChangePassword)
        {
            var user = await _userService.FindByUsernameOrEmailAsync(userChangePassword);

            if (user != null && VerifyUserPassword(userChangePassword.Password, user.Password))
            {
                if (userChangePassword.NewPassword == userChangePassword.ConfirmPassword)
                {
                    user.Password = EncryptionService.Hash(userChangePassword.NewPassword);

                    await _userService.UpdateAsync(user);

                    return Ok("Password changed successfully");
                }

                return BadRequest("The new password and confirm password do not match");
            }

            return BadRequest("Invalid username or password");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordDto userForgotPassword)
        {
            var user = await _userService.FindByEmailAsync(userForgotPassword.Email);

            if (user != null)
            {
                var newPassword = AuthService.GenerateRandomPassword();

                user.Password = EncryptionService.Hash(newPassword);

                await _userService.UpdateAsync(user);

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