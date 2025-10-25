using G3MWL.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace G3MWL.Services
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _client;

        public PatientService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("G3MWLApi");
        }

        private void AttachToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<Patient>> GetAllPatientsAsync(string token)
        {
            AttachToken(token);
            var response = await _client.GetAsync("api/patients");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Patient>>() ?? new List<Patient>();
        }

        public async Task<Patient?> GetPatientByIdAsync(string id, string token)
        {
            AttachToken(token);
            var response = await _client.GetAsync($"api/patients/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Patient>();
        }

        public async Task<bool> CreatePatientAsync(Patient patient, string token)
        {
            AttachToken(token);
            var response = await _client.PostAsJsonAsync("api/patients", patient);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddReportAsync(string id, WeeklyReport report, string token)
        {
            AttachToken(token);
            var response = await _client.PostAsJsonAsync($"api/patients/{id}/reports", report);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePatientAsync(string id, string token)
        {
            AttachToken(token);
            var response = await _client.DeleteAsync($"api/patients/{id}");
            return response.IsSuccessStatusCode;
        }

        public int GetTotalPatients()
        {
            try
            {
                var response = _client.GetAsync("api/patients").Result;
                if (!response.IsSuccessStatusCode) return 0;

                var patients = response.Content.ReadFromJsonAsync<List<Patient>>().Result;
                return patients?.Count ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}








