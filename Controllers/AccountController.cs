using G3MWL.Models;
using G3MWL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace G3MWL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _auth;

        public AccountController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var authResult = await _auth.LoginAsync(model);
            if (authResult == null || string.IsNullOrEmpty(authResult.Token))
            {
                ModelState.AddModelError("", "Invalid login");
                return View(model);
            }

            HttpContext.Session.SetString("AuthToken", authResult.Token);
            HttpContext.Session.SetString("UserEmail", authResult.User.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authResult.User.Email),
                new Claim("AccessToken", authResult.Token)
            };

            var identity = new ClaimsIdentity(claims, "SessionScheme");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("SessionScheme", principal);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _auth.RegisterAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Registration failed");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("SessionScheme");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}




