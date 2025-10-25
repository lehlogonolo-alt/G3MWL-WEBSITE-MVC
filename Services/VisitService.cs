using G3MWL.Models;
using System.Net.Http.Json;

namespace G3MWL.Services
{
    public class VisitService : IVisitService
    {
        private readonly HttpClient _client;

        public VisitService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("G3MWLApi");
        }

        public List<MonthlyVisitDto> GetMonthlyVisits(string siteName)
        {
            var response = _client.GetAsync($"api/visits/monthly?siteName={siteName}").Result;
            if (!response.IsSuccessStatusCode) return new List<MonthlyVisitDto>();

            var visits = response.Content.ReadFromJsonAsync<List<MonthlyVisitDto>>().Result;
            return visits ?? new List<MonthlyVisitDto>();
        }
    }
}


