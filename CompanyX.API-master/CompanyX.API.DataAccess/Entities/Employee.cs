using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyX.API.DataAccess.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime EmploymentDate { get; set; }

        public Employee? Boss { get; set; }

        public decimal CurentSalary { get; set; }
        
        [ForeignKey("HomeAddress")]
        public Guid HomeAddressId { get; set; }

        public EmployeeHomeAddress HomeAddress { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }

        public Role Role { get; set; }
    }
}
