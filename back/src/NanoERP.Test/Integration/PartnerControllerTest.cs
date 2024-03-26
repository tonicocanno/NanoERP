using MongoDB.Bson;
using NanoERP.API.Domain.Entities;
using NanoERP.API.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace NanoERP.Test.Integration
{
    public class PartnerControllerTest
    {
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;
        private readonly HttpClient _client;

        public PartnerControllerTest()
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

        private static Partner GetPartnerToTest()
        {
            var guid = Guid.NewGuid().ToString();

            return new Partner()
            {
                Id = ObjectId.GenerateNewId(),
                Type = PartnerType.Customer,
                Name = $"TestPartner{guid}",
                Email = $"testpartner{guid}@email.com",
                Phone1 = "123456789",
                Phone2 = "987654321"
            };
        }

        private static PartnerAddress GetAddessToTest()
        {
            var guid = Guid.NewGuid().ToString();

            return new PartnerAddress()
            {
                Id = ObjectId.GenerateNewId(),
                Street = $"TestStreet{guid}",
                Number = "123",
                City = "TestCity",
                State = "TS",
                Country = "TestCountry",
                PostalCode = "12345678"
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
        public async Task Get_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/partners");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetById_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var partner = GetPartnerToTest();

            var response = await _client.PostAsJsonAsync("/api/partners", partner);
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject(jsonResponseString);
            var id = (jsonResponse as JObject)?.GetValue("_id")?.ToString();

            var responseGet = await _client.GetAsync($"/api/partners/{id}");
            responseGet.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var partner = GetPartnerToTest();

            var response = await _client.PostAsJsonAsync("/api/partners", partner);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var partner = GetPartnerToTest();

            var response = await _client.PostAsJsonAsync("/api/partners", partner);
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject(jsonResponseString);
            var id = (jsonResponse as JObject)?.GetValue("_id")?.ToString();

            var responseDelete = await _client.DeleteAsync($"/api/partners/{id}");
            responseDelete.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddAddress_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var partner = GetPartnerToTest();

            var response = await _client.PostAsJsonAsync("/api/partners", partner);
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject(jsonResponseString);
            var id = (jsonResponse as JObject)?.GetValue("_id")?.ToString();

            var address = GetAddessToTest();

            var responseAddAddress = await _client.PostAsJsonAsync($"/api/partners/{id}/addresses", address);
            responseAddAddress.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RemoveAddress_ShouldReturnSuccessStatusCode()
        {
            var registrationDto = GetUserToTest();
            var token = await LoginAndGetTokenAsync(registrationDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var partner = GetPartnerToTest();

            var response = await _client.PostAsJsonAsync("/api/partners", partner);
            response.EnsureSuccessStatusCode();

            var jsonResponseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject(jsonResponseString);
            var id = (jsonResponse as JObject)?.GetValue("_id")?.ToString();

            var address = GetAddessToTest();

            var responseAddAddress = await _client.PostAsJsonAsync($"/api/partners/{id}/addresses", address);
            responseAddAddress.EnsureSuccessStatusCode();

            var jsonResponseAddressString = await responseAddAddress.Content.ReadAsStringAsync();
            var jsonResponseAddress = JsonConvert.DeserializeObject(jsonResponseAddressString);
            var addressId = (jsonResponseAddress as JObject)?.GetValue("_id")?.ToString();

            var responseRemoveAddress = await _client.DeleteAsync($"/api/partners/{id}/addresses/{addressId}");
            responseRemoveAddress.EnsureSuccessStatusCode();
        }
    }
}
