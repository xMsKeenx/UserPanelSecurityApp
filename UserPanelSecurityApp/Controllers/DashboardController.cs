using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserPanelSecurityApp.Data;
using UserPanelSecurityApp.Models;

namespace UserPanelSecurityApp.Controllers
{
    [Authorize] // Restricts access to authenticated accounts
    public class DashboardController : Controller
    {
        private readonly SecurityDbContext _context;

        public DashboardController(SecurityDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<IActionResult> Index()
        {
            int userId = GetCurrentUserId();
            var notes = await _context.UserNotes
                .Where(n => n.AppUserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notes);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction(nameof(Index));
            }

            var note = new UserNote
            {
                AppUserId = GetCurrentUserId(),
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserNotes.Add(note);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}