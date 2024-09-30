using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyX.API.DataAccess.Entities
{
    public class EmployeeHomeAddress
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

        public List<Employee> Employees { get; set; }
    }
}



