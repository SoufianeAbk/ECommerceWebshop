using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceWebshop.Data;
using ECommerceWebshop.Models;

namespace ECommerceWebshop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var totalUsers = await _userManager.Users.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);
            var recentOrders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();

            ViewData["TotalUsers"] = totalUsers;
            ViewData["TotalOrders"] = totalOrders;
            ViewData["TotalRevenue"] = totalRevenue;
            ViewData["RecentOrders"] = recentOrders;

            return View();
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    PhoneNumber = user.PhoneNumber ?? "",
                    RegistrationDate = user.RegistrationDate,
                    Roles = string.Join(", ", roles)
                });
            }

            return View(userViewModels);
        }

        // GET: Admin/UserDetails/5
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Email == user.Email)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            ViewData["User"] = user;
            ViewData["Roles"] = string.Join(", ", roles);
            ViewData["Orders"] = orders;

            return View(user);
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Don't allow deleting your own account
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id == id)
            {
                TempData["Error"] = "Je kunt je eigen account niet verwijderen.";
                return RedirectToAction(nameof(Users));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Gebruiker succesvol verwijderd.";
            }
            else
            {
                TempData["Error"] = "Er is een fout opgetreden bij het verwijderen van de gebruiker.";
            }

            return RedirectToAction(nameof(Users));
        }
    }

    // ViewModel voor gebruikerslijst
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string Roles { get; set; } = string.Empty;
    }
}