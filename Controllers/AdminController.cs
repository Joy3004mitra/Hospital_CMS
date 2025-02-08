using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Dashboard");
            }
            AdminLogin login = new AdminLogin();
            return View(login);
        }

        [HttpPost]
        public IActionResult Login(AdminLogin login)
        {
            var loginData = _context.AdminLogin.Where(x => (x.EmailId == login.EmailId) && (x.Passwd == login.Passwd)).FirstOrDefault();

            if (loginData != null)
            {
                HttpContext.Session.SetString("UserEmail", login.EmailId);
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return View();
            }
            return RedirectToAction("Login");
        }
        public IActionResult AppointmentHistory()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                var appointmentHistory = _context.AppointmentHistories.Where(x => x.TagDelete == 0).ToList();
                return View(appointmentHistory);
            }
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session
            return RedirectToAction("Login");
        }
    }
}
