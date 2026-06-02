using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserPanelSecurityApp.Data;

namespace UserPanelSecurityApp.Controllers
{
    [Authorize(Roles = "Admin")] // Authorization check for Admin security policy clearing
    public class AdminController : Controller
    {
        private readonly SecurityDbContext _context;

        public AdminController(SecurityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsers = await _context.AppUsers.CountAsync();
            var totalNotes = await _context.UserNotes.CountAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalNotes = totalNotes;

            return View();
        }
    }
}