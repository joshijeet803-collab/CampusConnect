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

            // Seed initial sample notices if database is empty
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

        // GET: Notice Board List Page
        public IActionResult Index()
        {
            var notices = _context.Notices.OrderByDescending(n => n.PostedDate).ToList();
            return View(notices);
        }

        // GET: Create Notice Form Page
        public IActionResult CreateNotice()
        {
            return View();
        }

        // POST: Save New Notice to Database
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
        // GET: Notice Details Page
        public IActionResult Details(int id)
        {
            var notice = _context.Notices.FirstOrDefault(n => n.Id == id);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
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