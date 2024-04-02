using CloneKun.Data;
using CloneKun.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CloneKun.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Register register = await _context.Register.FirstOrDefaultAsync();
            if (register == null)
            {              
                return NotFound();
            }

            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Cộng thêm 8 vào số người đăng ký
                    int incrementValue = 8;
                    _context.Database.ExecuteSqlInterpolated($"UPDATE Register SET NumberOfRegistrations = NumberOfRegistrations + {incrementValue}");

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý ngoại lệ nếu có
                    ModelState.AddModelError("", "Error occurred while saving changes.");
                    return RedirectToAction(nameof(Error));
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
