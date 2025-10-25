using G3MWL.Models;
using System.Net.Http.Json;

namespace G3MWL.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;

        public AuthService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("G3MWLApi");
        }

        public async Task<AuthResponse?> LoginAsync(UserLogin login)
        {
            var response = await _client.PostAsJsonAsync("api/auth/login", login);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }


        public async Task<bool> RegisterAsync(UserRegister register)
        {
            var response = await _client.PostAsJsonAsync("api/auth/register", register);
            return response.IsSuccessStatusCode;
        }
    }
}


