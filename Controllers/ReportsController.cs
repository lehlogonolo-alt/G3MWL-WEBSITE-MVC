using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using G3MWL.Services;
using G3MWL.Documents;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using QuestPDF.Fluent;

namespace G3MWL.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IPatientService _patientService;

        public ReportsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: /Reports?searchTerm=...
        public async Task<IActionResult> Index(string searchTerm)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var patients = await _patientService.GetAllPatientsAsync(token);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                patients = patients
                    .Where(p => p.FirstName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(patients);
        }

        // GET: /Reports/Report/{id}
        public async Task<IActionResult> Report(string id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var patient = await _patientService.GetPatientByIdAsync(id, token);
            return View(patient);
        }

        // POST: /Reports/Export
        [HttpPost]
        public async Task<IActionResult> Export(string id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var patient = await _patientService.GetPatientByIdAsync(id, token);

            var document = new PatientReportDocument(patient);
            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", $"PatientReport_{patient._id}.pdf");
        }
    }
}






