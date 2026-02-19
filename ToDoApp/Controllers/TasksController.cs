using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Helper method (clean way to get current user ID)
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            // or: return _userManager.GetUserId(User)!;
        }

        // GET: Tasks (only user's task)
        public async Task<IActionResult> Index(string? filter = "all")
        {
            var userId = GetCurrentUserId();

            var tasks = _context.TaskItems
                .Where(t => t.UserId == userId);

            if (filter == "completed")
                tasks = tasks.Where(t => t.IsCompleted);
            else if (filter == "pending")
                tasks = tasks.Where(t => !t.IsCompleted);

            return View(await tasks.OrderByDescending(t => t.CreatedDate).ToListAsync());
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.UserId = GetCurrentUserId();
                taskItem.CreatedDate = DateTime.UtcNow;
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == GetCurrentUserId());
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            var existing = await _context.TaskItems
                .AnyAsync(t => t.Id == id && t.UserId == GetCurrentUserId());

            if (!existing) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == GetCurrentUserId());
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetCurrentUserId());
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tasks/Details/6
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == GetCurrentUserId());

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: Toggle complete (you can add a nice AJAX button later)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetCurrentUserId());

            if (task == null)
            {
                return NotFound();           // ← important: don't create anything
            }

            task.IsCompleted = !task.IsCompleted;   // toggle
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));   // Post-Redirect-Get pattern
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}
