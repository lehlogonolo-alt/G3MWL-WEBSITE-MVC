using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using G3MWL.Services;

namespace G3MWL.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuditService _auditService;
        private readonly IVisitService _visitService;

        public DashboardController(
            IHttpClientFactory httpClientFactory,
            IAuditService auditService,
            IVisitService visitService)
        {
            _httpClientFactory = httpClientFactory;
            _auditService = auditService;
            _visitService = visitService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var userEmail = ExtractEmailFromToken(token);
            var lastActivity = _auditService.GetLastActivity(userEmail);

            // Welcome message data
            ViewBag.UserName = userEmail?.Split('@')[0] ?? "Admin";
            ViewBag.LastSeen = string.IsNullOrWhiteSpace(lastActivity) ? "recently" : lastActivity;
            ViewBag.RecentActivities = _auditService.GetRecentActivities(5);

            // Fetch dashboard metrics from Node.js backend
            var client = _httpClientFactory.CreateClient("G3MWLApi");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var response = await client.GetAsync("api/dashboard");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(json);

                    ViewBag.PatientCount = (int?)data["totalPatients"] ?? 0;
                    ViewBag.SideEffectCount = (int?)data["sideEffectCount"] ?? 0;
                    ViewBag.AverageWeightLossPercentage = (double?)data["averageWeightLossPercentage"] ?? 0.0;
                }
                else
                {
                    ViewBag.PatientCount = 0;
                    ViewBag.SideEffectCount = 0;
                    ViewBag.AverageWeightLossPercentage = 0.0;
                }
            }
            catch
            {
                ViewBag.PatientCount = 0;
                ViewBag.SideEffectCount = 0;
                ViewBag.AverageWeightLossPercentage = 0.0;
            }

            // Site visit analytics
            var visits = _visitService.GetMonthlyVisits("G3MWL");
            ViewBag.VisitLabels = visits.Select(v => v.Month).ToList();
            ViewBag.VisitData = visits.Select(v => v.Count).ToList();

            return View();
        }

        private string ExtractEmailFromToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return "unknown";

            try
            {
                var payload = token.Split('.')[1];
                var padded = PadBase64(payload);
                var jsonBytes = System.Convert.FromBase64String(padded);
                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                return claims != null && claims.ContainsKey("email") ? claims["email"].ToString() : "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        private string PadBase64(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }
    }
}











