using G3MWL.Models;
using G3MWL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G3MWL.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly ISideEffectService _sideEffectService;

        public PatientController(IPatientService patientService, ISideEffectService sideEffectService)
        {
            _patientService = patientService;
            _sideEffectService = sideEffectService;
        }

        // GET: /Patient
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var patients = await _patientService.GetAllPatientsAsync(token);

            if (!string.IsNullOrEmpty(searchString))
            {
                string lowerSearch = searchString.ToLower();
                patients = patients.Where(p =>
                    p.FirstName.ToLower().Contains(lowerSearch) ||
                    p.LastName.ToLower().Contains(lowerSearch) ||
                    p._id.ToLower().Contains(lowerSearch)).ToList();
            }

            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["CurrentFilter"] = searchString;

            patients = sortOrder switch
            {
                "name_desc" => patients.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName).ToList(),
                "Date" => patients.OrderBy(p => p.TreatmentStartDate).ToList(),
                "date_desc" => patients.OrderByDescending(p => p.TreatmentStartDate).ToList(),
                "Id" => patients.OrderBy(p => p._id).ToList(),
                "id_desc" => patients.OrderByDescending(p => p._id).ToList(),
                _ => patients.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList()
            };

            return View(patients);
        }

        // GET: /Patient/Create
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var sideEffects = await _sideEffectService.GetAllSideEffectsAsync(token);
            ViewBag.SideEffectsList = sideEffects.Select(se => new SelectListItem { Value = se.Name, Text = se.Name }).ToList();

            return View(new CreatePatientViewModel { TreatmentStartDate = DateTime.Today });
        }

        // POST: /Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePatientViewModel viewModel)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var sideEffects = await _sideEffectService.GetAllSideEffectsAsync(token);
            ViewBag.SideEffectsList = sideEffects.Select(se => new SelectListItem { Value = se.Name, Text = se.Name }).ToList();

            if (!ModelState.IsValid) return View(viewModel);

            var newPatient = new Patient
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Age = viewModel.Age,
                Gender = viewModel.Gender,
                TreatmentStartDate = viewModel.TreatmentStartDate,
                StartingWeight = viewModel.StartingWeight,
                WeeklyReports = new List<WeeklyReport>
                {
                    new WeeklyReport
                    {
                        WeekNumber = 1,
                        Weight = viewModel.CurrentWeight,
                        Dosage = viewModel.Dosage.ToString("F1"),
                        SideEffects = viewModel.SideEffects ?? new List<string>(),
                        Results = viewModel.Results.ToString("F1"),
                        Notes = viewModel.Notes
                    }
                }
            };

            var success = await _patientService.CreatePatientAsync(newPatient, token);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to create patient.");
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        // GET: /Patient/Report/{id}
        public async Task<IActionResult> Report(string id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var patient = await _patientService.GetPatientByIdAsync(id, token);
            if (patient == null) return NotFound();

            return View(patient);
        }

        // GET: /Patient/AddReport/{id}
        public async Task<IActionResult> AddReport(string id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var patient = await _patientService.GetPatientByIdAsync(id, token);
            if (patient == null) return NotFound();

            var lastReport = patient.WeeklyReports.OrderByDescending(r => r.WeekNumber).FirstOrDefault();
            ViewData["LastReport"] = lastReport;
            ViewData["Patient"] = patient;

            var sideEffects = await _sideEffectService.GetAllSideEffectsAsync(token);
            ViewBag.SideEffectsList = sideEffects.Select(se => new SelectListItem { Value = se.Name, Text = se.Name }).ToList();

            return View(new WeeklyReport
            {
                WeekNumber = lastReport != null ? lastReport.WeekNumber + 1 : 1,
                Dosage = lastReport?.Dosage ?? "0"
            });
        }

        // POST: /Patient/AddReport/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReport(string id, WeeklyReport newReport)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var patient = await _patientService.GetPatientByIdAsync(id, token);
            if (patient == null) return NotFound();

            if (!ModelState.IsValid)
            {
                var sideEffects = await _sideEffectService.GetAllSideEffectsAsync(token);
                ViewBag.SideEffectsList = sideEffects.Select(se => new SelectListItem { Value = se.Name, Text = se.Name }).ToList();
                ViewData["Patient"] = patient;
                return View(newReport);
            }

            if (newReport.Weight.HasValue)
                newReport.Weight = Math.Round(newReport.Weight.Value, 1);

            var success = await _patientService.AddReportAsync(id, newReport, token);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to add report.");
                return View(newReport);
            }

            return RedirectToAction("Report", new { id });
        }

        // POST: /Patient/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var success = await _patientService.DeletePatientAsync(id, token);

            if (!success)
            {
                TempData["Error"] = "Failed to delete patient.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Patient deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}






