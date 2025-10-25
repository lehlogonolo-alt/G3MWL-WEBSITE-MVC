using G3MWL.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace G3MWL.Services
{
    public class SideEffectService : ISideEffectService
    {
        private readonly HttpClient _client;

        public SideEffectService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("G3MWLApi");
        }

        private void AttachToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<SideEffect>> GetAllSideEffectsAsync(string token)
        {
            AttachToken(token);
            var response = await _client.GetAsync("api/side-effects");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SideEffect>>() ?? new List<SideEffect>();
        }

        public async Task<bool> CreateSideEffectAsync(SideEffect sideEffect, string token)
        {
            AttachToken(token);
            var response = await _client.PostAsJsonAsync("api/side-effects", sideEffect);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSideEffectAsync(string id, string token)
        {
            AttachToken(token);
            var response = await _client.DeleteAsync($"api/side-effects/{id}");
            return response.IsSuccessStatusCode;
        }

        public int GetTotalSideEffects()
        {
            try
            {
                var response = _client.GetAsync("api/side-effects").Result;
                if (!response.IsSuccessStatusCode) return 0;

                var effects = response.Content.ReadFromJsonAsync<List<SideEffect>>().Result;
                return effects?.Count ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}






