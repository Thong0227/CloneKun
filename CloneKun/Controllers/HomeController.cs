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
        public async Task<IActionResult> Edit([Bind("NumberOfRegistrations")] Register register)
        {
            if (ModelState.IsValid)
            {
                  // Cộng thêm 8 vào số người đăng ký
                int incrementValue = 8;
                _context.Database.ExecuteSqlInterpolated($"UPDATE Register SET NumberOfRegistrations = NumberOfRegistrations + {incrementValue}");
                int updatedRegistrations = await _context.Register.Select(r => r.NumberOfRegistrations).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
                    return Json(new { success = true, NumberOfRegistrations = updatedRegistrations });
     
            }
			return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors), });
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
