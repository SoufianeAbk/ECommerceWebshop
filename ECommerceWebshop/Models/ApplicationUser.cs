using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWebshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation property voor orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}