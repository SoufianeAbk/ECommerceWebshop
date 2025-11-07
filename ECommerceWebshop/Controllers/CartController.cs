using Microsoft.AspNetCore.Mvc;
using ECommerceWebshop.Data;
using ECommerceWebshop.Models;
using ECommerceWebshop.Helpers;
using ECommerceWebshop.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebshop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly InvoiceService _invoiceService;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ApplicationDbContext context,
            InvoiceService invoiceService,
            ILogger<CartController> logger)
        {
            _context = context;
            _invoiceService = invoiceService;
            _logger = logger;
        }

        // GET: Cart
        public IActionResult Index()
        {
            var cart = GetCart();
            ViewData["CartTotal"] = cart.Sum(item => item.Total);
            return View(cart);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/RemoveFromCart
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/ClearCart
        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            TempData["Success"] = "Alle items zijn uit de winkelwagen verwijderd.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Cart/Checkout
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["CartItems"] = cart;
            ViewData["CartTotal"] = cart.Sum(item => item.Total);

            return View(new Order());
        }

        // POST: Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                // Generate order number
                order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
                order.OrderDate = DateTime.Now;
                order.TotalAmount = cart.Sum(item => item.Total);
                order.Status = "Processing";

                // Add order items
                foreach (var cartItem in cart)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        ProductName = cartItem.ProductName,
                        Price = cartItem.Price,
                        Quantity = cartItem.Quantity
                    };
                    order.OrderItems.Add(orderItem);

                    // Update stock
                    var product = await _context.Products.FindAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        product.Stock -= cartItem.Quantity;
                    }
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // ========== NIEUWE CODE: GENEREER PDF FACTUUR ==========
                try
                {
                    string invoicePath = _invoiceService.GenerateInvoice(order);
                    TempData["InvoicePath"] = invoicePath;
                    TempData["Success"] = $"Bestelling succesvol geplaatst! Uw factuur is gegenereerd.";
                }
                catch (Exception ex)
                {
                    // Log de fout maar ga door met de bestelling
                    TempData["Warning"] = $"Bestelling succesvol, maar factuur kon niet worden gegenereerd: {ex.Message}";
                }
                // ========================================================

                // Clear cart
                HttpContext.Session.Remove("Cart");

                return RedirectToAction(nameof(OrderConfirmation), new { id = order.Id });
            }

            ViewData["CartItems"] = cart;
            ViewData["CartTotal"] = cart.Sum(item => item.Total);

            return View(order);
        }

        // GET: Cart/OrderConfirmation
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Check of er een factuur bestaat voor deze bestelling
            ViewData["InvoiceExists"] = _invoiceService.InvoiceExists(order.OrderNumber);

            return View(order);
        }

        // GET: Cart/DownloadInvoice
        public IActionResult DownloadInvoice(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return BadRequest("Bestelnummer is verplicht");
            }

            string invoicePath = _invoiceService.GetInvoicePath(orderNumber);

            if (string.IsNullOrEmpty(invoicePath) || !System.IO.File.Exists(invoicePath))
            {
                TempData["Error"] = "Factuur niet gevonden";
                return RedirectToAction(nameof(Index));
            }

            var fileBytes = System.IO.File.ReadAllBytes(invoicePath);
            var fileName = Path.GetFileName(invoicePath);

            return File(fileBytes, "application/pdf", fileName);
        }

        // GET: Cart/GetCartCount
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(cart.Sum(item => item.Quantity));
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson("Cart", cart);
        }
    }
}