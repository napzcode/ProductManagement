using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.Controllers
{
    public class AuthController : Controller
    {
        private const string AdminPassword = "admin123"; // Change this to your own password

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string password, string? returnUrl)
        {
            if (password == AdminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return Redirect(returnUrl ?? "/");
            }
            ViewData["Error"] = "Wrong password.";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Products");
        }
    }
}
