using Microsoft.EntityFrameworkCore;
using ECommerceWebshop.Models;

namespace ECommerceWebshop.Data
{
    // ✅ Deze class is nu alleen een alias voor backwards compatibility
    // Alle functionaliteit zit in ApplicationIdentityDbContext
    public class ApplicationDbContext : ApplicationIdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }
    }
}