using Microsoft.Extensions.Configuration;
using NanoERP.API.Domain.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace NanoERP.Test
{
    public class AuthControllerTest
    {
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;
        private readonly HttpClient _client;

        public AuthControllerTest()
        {
            _customWebApplicationFactory = new CustomWebApplicationFactory<Program>();
            _client = _customWebApplicationFactory.CreateClient();
        }

        private static UserRegistrationDto GetUserToTest()
        {
            var guid = Guid.NewGuid().ToString();

            return new UserRegistrationDto
            {
                Email = $"test{guid}@email.com",
                Username = $"TestUser{guid}",
                Password = "test123",
                Name = "Test",
                Surname = "User"
            };
        }

        private async Task<string> LoginAndGetTokenAsync(UserRegistrationDto registrationDto)
        {
            await _client.PostAsJsonAsync("/api/auth/register", registrationDto);

            var responseWithUsername = await _client.PostAsJsonAsync("/api/auth/login",
                new UserLoginDto { Username = registrationDto.Username, Password = registrationDto.Password });
            responseWithUsername.EnsureSuccessStatusCode();

            var responseWithEmail = await _client.PostAsJsonAsync("/api/auth/login",
                new UserLoginDto { Email = registrationDto.Email, Password = registrationDto.Password });
            responseWithEmail.EnsureSuccessStatusCode();

            var jsonResponseString = await responseWithUsername.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject(jsonResponseString);
            var token = (jsonResponse as JObject)?.GetValue("token")?.ToString();

            return token ?? null!;
        }

        [Fact]
        public async Task Register_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();

            var response = await _client.PostAsJsonAsync("/api/auth/register", registrationDto);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Login_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            Assert.NotNull(token);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync("/api/auth/change-password",
                new UserChangePasswordDto
                {
                    Username = registrationDto.Username,
                    Password = registrationDto.Password,
                    NewPassword = "newPassword123",
                    ConfirmPassword = "newPassword123"
                });
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ChangePassword_SucesfullyChangedPassword()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync("/api/auth/change-password",
                new UserChangePasswordDto
                {
                    Username = registrationDto.Username,
                    Password = registrationDto.Password,
                    NewPassword = "newPassword123",
                    ConfirmPassword = "newPassword123"
                });

            if (response.IsSuccessStatusCode)
            {
                var responseAfterChange = await _client.PostAsJsonAsync("/api/auth/login",
                    new UserLoginDto { Username = registrationDto.Username, Password = "newPassword123" });
                responseAfterChange.EnsureSuccessStatusCode();
            }
        }
    }
}
