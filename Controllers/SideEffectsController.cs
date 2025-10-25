using G3MWL.Models;
using G3MWL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace G3MWL.Controllers
{
    public class SideEffectsController : Controller
    {
        private readonly ISideEffectService _sideEffectService;

        public SideEffectsController(ISideEffectService sideEffectService)
        {
            _sideEffectService = sideEffectService;
        }

        // GET: /SideEffects
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var sideEffects = await _sideEffectService.GetAllSideEffectsAsync(token);
            return View(sideEffects);
        }

        // GET: /SideEffects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /SideEffects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Severity")] SideEffect sideEffect)
        {
            Debug.WriteLine($"📥 Incoming form data: Name={sideEffect.Name}, Severity={sideEffect.Severity}");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("❌ ModelState is invalid");
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Debug.WriteLine($"❌ Field '{entry.Key}' error: {error.ErrorMessage}");
                    }
                }
                return View(sideEffect);
            }

            var token = HttpContext.Session.GetString("AuthToken");
            var success = await _sideEffectService.CreateSideEffectAsync(sideEffect, token);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to create side effect.");
                Debug.WriteLine("❌ Backend creation failed");
                return View(sideEffect);
            }

            Debug.WriteLine("✅ Side effect created successfully");
            return RedirectToAction(nameof(Index));
        }

        // POST: /SideEffects/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            var success = await _sideEffectService.DeleteSideEffectAsync(id, token);

            if (!success)
            {
                TempData["Error"] = "Failed to delete side effect.";
                Debug.WriteLine($"❌ Failed to delete side effect with ID: {id}");
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Side effect deleted successfully.";
            Debug.WriteLine($"✅ Side effect deleted: {id}");
            return RedirectToAction(nameof(Index));
        }
    }
}






