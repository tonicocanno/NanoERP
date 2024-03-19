using System.Net.Http.Headers;
using System.Net.Http.Json;
using MongoDB.Bson;
using NanoERP.API.Domain.Entities;
using NanoERP.API.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NanoERP.Test.Integration
{
    public class ProductControllerTest
    {
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;
        private readonly HttpClient _client;

        public ProductControllerTest()
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

        private static Product GetProductToTest()
        {
            var guid = Guid.NewGuid().ToString();

            return new Product()
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"TestProduct{guid}",
                Description = "TestDescription",
                Price = 10,
                Stock = 100
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
        public async Task GetById_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var product = GetProductToTest();

            var response = await _client.PostAsJsonAsync("/api/products", product);
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject(jsonResponseString);
            var id = (jsonResponse as JObject)?.GetValue("id")?.ToString();

            var responseGet = await _client.GetAsync($"/api/products/{id}");
            responseGet.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var responseGet = await _client.GetAsync("/api/products");
            responseGet.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var product = GetProductToTest();

            var response = await _client.PostAsJsonAsync("/api/products", product);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var product = GetProductToTest();

            var response = await _client.PostAsJsonAsync("/api/products", product);
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject(jsonResponseString);
            var id = (jsonResponse as JObject)?.GetValue("_id")?.ToString();

            var responseDelete = await _client.DeleteAsync($"/api/products/{id}");
            responseDelete.EnsureSuccessStatusCode();
        }
    }
}