using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyX.API.DataAccess.Entities
{
    public class CustomerHomeAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string StreetName { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int PostalCode { get; set; }
    }
}
