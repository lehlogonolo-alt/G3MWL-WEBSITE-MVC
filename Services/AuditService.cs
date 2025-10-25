using G3MWL.Models;
using System.Net.Http.Json;

namespace G3MWL.Services
{
    public class AuditService : IAuditService
    {
        private readonly HttpClient _client;

        public AuditService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("G3MWLApi");
        }

        public int GetLoginCount(string email)
        {
            var response = _client.GetAsync($"api/activity/loginCount?email={email}").Result;
            if (!response.IsSuccessStatusCode) return 0;

            var count = response.Content.ReadFromJsonAsync<int>().Result;
            return count;
        }

        public string GetLastActivity(string email)
        {
            var response = _client.GetAsync($"api/activity/lastActivity?email={email}").Result;
            if (!response.IsSuccessStatusCode) return "No activity recorded";

            var activityText = response.Content.ReadAsStringAsync().Result;
            return string.IsNullOrWhiteSpace(activityText) ? "No activity recorded" : activityText;
        }

        public List<UserActivity> GetRecentActivities(int count)
        {
            var response = _client.GetAsync($"api/activity/recent?count={count}").Result;
            if (!response.IsSuccessStatusCode) return new List<UserActivity>();

            var activities = response.Content.ReadFromJsonAsync<List<UserActivity>>().Result;
            return activities ?? new List<UserActivity>();
        }
    }
}





