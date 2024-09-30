
using System.ComponentModel.DataAnnotations;

namespace CompanyX.API.DataAccess.Models
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Street name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Street name must be between 1 and 100 characters long.")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "House number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "House number must be greater than 0.")]
        public int HouseNumber { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "City must be between 1 and 50 characters long.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [Range(1000, 5000, ErrorMessage = "Postal code must be between 10000 and 99999.")]
        public int PostalCode { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        public string? CardNumber { get; set; }
    }

}
