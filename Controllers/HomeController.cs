using System.Diagnostics;
using CampusConnect.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

            if (!_context.Notices.Any())
            {
                _context.Notices.AddRange(
                    new Notice
                    {
                        Title = "Mid-Sem Examination Schedule Published",
                        Category = "Exams",
                        TargetDepartment = "Computer Engineering",
                        Description = "All B.Tech Semester 5 students are hereby informed that the mid-sem timetable has been uploaded.",
                        PostedBy = "Admin",
                        PostedDate = DateTime.Now.AddDays(-2)
                    },
                    new Notice
                    {
                        Title = "Annual Sports Week Registration Open",
                        Category = "Sports",
                        TargetDepartment = "All Departments",
                        Description = "Cricket, Football, and Table Tennis registrations are now active. Register before end of this week.",
                        PostedBy = "Sports Club",
                        PostedDate = DateTime.Now.AddDays(-1)
                    }
                );
                _context.SaveChanges();
            }
        }

        public IActionResult Index()
        {
            var notices = _context.Notices.OrderByDescending(n => n.PostedDate).ToList();
            return View(notices);
        }

        public IActionResult CreateNotice()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNotice(Notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.PostedDate = DateTime.Now;
                notice.PostedBy = "Admin";

                _context.Notices.Add(notice);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(notice);
        }

        public IActionResult Details(int id)
        {
            var notice = _context.Notices.FirstOrDefault(n => n.Id == id);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
        }

        // POST: Delete Notice from Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteNotice(int id)
        {
            var notice = _context.Notices.FirstOrDefault(n => n.Id == id);
            if (notice != null)
            {
                _context.Notices.Remove(notice);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email))
            {
                string displayName = email.Contains("@") ? email.Split('@')[0] : email;

                Response.Cookies.Append("UserRole", "Admin");
                Response.Cookies.Append("UserName", displayName);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserRole");
            Response.Cookies.Delete("UserName");
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}