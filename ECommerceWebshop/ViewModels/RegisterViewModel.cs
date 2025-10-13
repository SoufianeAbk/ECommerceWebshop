using System.ComponentModel.DataAnnotations;

namespace ECommerceWebshop.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [StringLength(100)]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achternaam is verplicht")]
        [StringLength(100)]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [StringLength(100, ErrorMessage = "Het {0} moet minimaal {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Telefoonnummer")]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "Adres")]
        public string? Address { get; set; }

        [StringLength(100)]
        [Display(Name = "Stad")]
        public string? City { get; set; }

        [StringLength(10)]
        [Display(Name = "Postcode")]
        public string? PostalCode { get; set; }
    }
}