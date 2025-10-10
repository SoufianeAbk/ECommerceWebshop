using System.Diagnostics;
using ECommerceWebshop.Models;
using ECommerceWebshop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["FeaturedProducts"] = await _context.Products
                .Include(p => p.Category)
                .Take(8)
                .ToListAsync();

            ViewData["Categories"] = await _context.Categories.ToListAsync();

            return View();
        }
    }
}